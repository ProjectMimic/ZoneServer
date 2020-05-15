using Microsoft.ClearScript;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace ZoneServer
{
    class WeightedRNG<TKey>
    {
        private Dictionary<TKey, int> items;
        private int totalWeight = 0;

        public int Count { get { return items.Count; } }

        public WeightedRNG(Dictionary<TKey, int> weightedItems)
        {
            items = weightedItems;
            totalWeight = items.Values.Sum();
        }

        public TKey Get()
        {
            int value = RNG.Get(totalWeight);
            foreach (KeyValuePair<TKey, int> entry in items)
            {
                if (entry.Value > value)
                {
                    return entry.Key;
                }
            }
            throw new InvalidOperationException("Attempting Get on empty weighted list");
        }

        public TKey Take()
        {
            TKey item = Get();
            Remove(item);
            return item;
        }

        public void Add(TKey item, int weight)
        {
            items.Add(item, weight);
            totalWeight += weight;
        }

        public void Remove(TKey item)
        {
            int weight = 0;
            if (items.TryGetValue(item, out weight))
            {
                totalWeight -= weight;
                items.Remove(item);
            }
        }
    }
}
