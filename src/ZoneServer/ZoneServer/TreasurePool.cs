using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace ZoneServer
{
    public enum ItemLotResult
    {
        Win = 1,
        WinError = 2,
        Lost = 3,
    };

    public class TreasurePoolItem
    {
        public UInt16 itemID = 0;
        public byte index = 0;
        public Timer timer = new Timer();
        public Dictionary<Entity, UInt16> lots = new Dictionary<Entity, UInt16>();
        public bool Empty { get { return itemID == 0; } }
    }

    class TreasurePool
    {
        static private int TreasurePoolMax = 10;
        static private int TreasurePoolTime = 300;
        static private UInt16 PassedLotValue = UInt16.MaxValue;

        private List<Entity> members = new List<Entity>();
        private List<TreasurePoolItem> items = new List<TreasurePoolItem>(TreasurePoolMax);
        // Priority of ItemFlags to avoid when replacing an existing item with a new one
        private ItemFlags[] replacePriority = new ItemFlags[] { ItemFlags.Rare | ItemFlags.Exclusive, ItemFlags.Exclusive, ItemFlags.None };

        public TreasurePool()
        {
            for (byte i = 0; i < TreasurePoolMax; ++i)
            {
                items[i].index = i;
            }
        }

        public void AddMember(Entity entity)
        {
            members.Add(entity);

            // TODO: Push 0xD2 (TreasureFindItemPacket) for all existing items to member
        }

        public void RemoveMember(Entity entity)
        {
            foreach (TreasurePoolItem item in items)
            {
                if (item.itemID != 0)
                {
                    item.lots.Remove(entity);
                }
            }

            members.Remove(entity);
        }

        public void AddItem(Entity monster, UInt16 itemID)
        {
            int targetIndex = items.FindIndex(item => item.Empty);
            if (targetIndex == -1)
            {
                foreach (ItemFlags flags in replacePriority)
                {
                    targetIndex = FindTargetIndex(flags);
                    if (targetIndex != -1)
                    {
                        break;
                    }
                }
                if (targetIndex == -1)
                {
                    targetIndex = 0;
                }
            }

            TreasurePoolItem item = items[targetIndex];
            if (!item.Empty)
            {
                DistributeItemAt(targetIndex);
            }

            item.itemID = itemID;
            item.timer.Reset(TreasurePoolMax);

            // TODO: Push 0xD2 (TreasureFindItemPacket) to members

            if (members.Count == 1)
            {
                // NOTE: This might not be desired for zone treasure pools?
                DistributeItemAt(targetIndex);
            }
        }

        public void LotItem(Entity entity, UInt16 itemIndex, UInt16 value)
        {
            TreasurePoolItem item = items[itemIndex];
            item.lots[entity] = value;

            Entity bestLotter = null;
            int bestLot = 0;
            foreach (var itemLot in item.lots)
            {
                if (itemLot.Value != PassedLotValue && itemLot.Value > bestLot)
                {
                    bestLotter = itemLot.Key;
                    bestLot = itemLot.Value;
                }
            }

            // TODO: Send 0xD3 (TreasureLotItemPacket) to all members

            if (items[itemIndex].lots.Count == members.Count)
            {
                DistributeItemAt(itemIndex);
            }
        }

        public void PassItem(Entity entity, UInt16 itemIndex)
        {
            LotItem(entity, itemIndex, PassedLotValue);
        }

        public bool CanLot(Entity entity, UInt16 itemIndex)
        {
            return items[itemIndex].lots.ContainsKey(entity);
        }

        public bool CanPass(Entity entity, UInt16 itemIndex)
        {
            UInt16 value = 0;
            items[itemIndex].lots.TryGetValue(entity, out value);
            return value != PassedLotValue;
        }

        public void Update(int elapsed)
        {
            foreach (TreasurePoolItem item in items)
            {
                if (!item.Empty && item.timer.Update(elapsed))
                {
                    DistributeItemAt(item.index);
                }
            }
        }

        private int FindTargetIndex(ItemFlags flags)
        {
            int targetIndex = -1;
            int soonestExpiration = TreasurePoolTime;
            foreach (TreasurePoolItem item in items)
            {
                if (!item.Empty && !Item.Get(item.itemID).Flags.HasFlag(flags) && soonestExpiration > item.timer.Remaining)
                {
                    targetIndex = item.index;
                    soonestExpiration = item.timer.Remaining;
                }
            }
            return targetIndex;
        }

        private void DistributeItemAt(int itemIndex)
        {
            TreasurePoolItem poolItem = items[itemIndex];
            Item item = Item.Get(poolItem.itemID);
            Entity winner = null;
            int bestLot = 0;
            foreach (var itemLot in poolItem.lots)
            {
                if (itemLot.Value != PassedLotValue && itemLot.Value > bestLot)
                {
                    winner = itemLot.Key;
                    bestLot = itemLot.Value;
                }
            }

            if (winner == null)
            {
                winner = members.Where(member => member.CanAddItem(item) && poolItem.lots.ContainsKey(member)).ToList().GetRandom();
            }

            if (winner.HasInventorySpace)
            {
                if (winner.AddItem(item))
                {
                    ProcessItemResult(winner, poolItem, ItemLotResult.Win);
                }
                else
                {
                    ProcessItemResult(winner, poolItem, ItemLotResult.WinError);
                }
            }
            else
            {
                ProcessItemResult(winner, poolItem, ItemLotResult.Lost);
            }
        }

        private void ProcessItemResult(Entity winner, TreasurePoolItem item, ItemLotResult result)
        {
            item.itemID = 0;
            item.timer.Stop();
            item.lots.Clear();
            // TODO: Send 0xD3 (TreasureLotItemPacket) with result
        }
    }
}
