using System;
using System.Collections.Generic;
using System.Linq;

namespace DarkwoodRandomizer.Plugin
{
    internal static class ExtensionMethods
    {
        internal static T RandomItem<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.ToArray()[UnityEngine.Random.Range(0, enumerable.Count())];
        }

        internal static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
