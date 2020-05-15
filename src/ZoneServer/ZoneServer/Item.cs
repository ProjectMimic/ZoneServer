using System;
using System.Collections.Generic;
using System.Text;

namespace ZoneServer
{
    [Flags]
    public enum ItemFlags
    {
        None = 0x0000,
        Wallhanging = 0x0001,
        UNKNOWN = 0x0002,
        MysteryBox = 0x0004, // Can be gained from Gobbie Mystery Box
        MogGarden = 0x0008, // Can use in Mog Garden
        Mail2Account = 0x0010, // CanSendPOL Polutils Value
        Inscribable = 0x0020,
        NoAuction = 0x0040,
        Scroll = 0x0080,
        Linkshell = 0x0100, // Linkshell Polutils Value
        CanUse = 0x0200,
        CanTradeNPC = 0x0400,
        CanEquip = 0x0800,
        NoSale = 0x1000,
        NoDelivery = 0x2000,
        Exclusive = 0x4000, // NoTradePC Polutils Value
        Rare = 0x8000,
    };
    public class Item
    {
        private static Dictionary<UInt16, Item> items;

        public ItemFlags Flags { get; private set; }

        public static Item Get(UInt16 itemID)
        {
            return items[itemID];
        }
    }
}
