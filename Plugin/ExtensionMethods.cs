using System.Collections.Generic;
using System.Linq;

namespace DarkwoodRandomizer.Plugin
{
    internal static class ExtensionMethods
    {
        internal static T RandomIndex<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.ToArray()[UnityEngine.Random.Range(0, enumerable.Count())];
        }
    }
}
