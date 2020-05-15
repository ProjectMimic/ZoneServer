using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ZoneServer
{
    public static class RNG
    {
        private static int counter = 0;
        private static readonly ThreadLocal<Random> threadRandom = new ThreadLocal<Random>(CreateRandom);

        public static Random CreateRandom()
        {
            int seed = (int)(DateTime.Now.Ticks & 0x0000FFFF) + counter;
            Interlocked.Increment(ref counter);
            return new Random(seed);
        }

        public static Random Instance { get { return threadRandom.Value; } }

        public static int Get(int max)
        {
            return Instance.Next(max);
        }
        public static int Get(int min, int max)
        {
            return Instance.Next(min, max);
        }
        public static double Get()
        {
            return Instance.NextDouble();
        }

        public static TKey GetWeighted<TKey>(Dictionary<TKey, int> weighted)
        {
            int value = RNG.Get(weighted.Values.Sum());
            foreach (KeyValuePair<TKey, int> entry in weighted)
            {
                if (entry.Value > value)
                {
                    return entry.Key;
                }
            }
            throw new InvalidOperationException("Attempting GetWeighted for empty dictionary");
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = RNG.Get(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static T GetRandom<T>(this IList<T> list)
        {
            return list[RNG.Get(list.Count)];
        }
    }
}
