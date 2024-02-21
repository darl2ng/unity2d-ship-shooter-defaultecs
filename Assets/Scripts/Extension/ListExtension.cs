using System;
using System.Collections.Generic;

namespace Extension
{
    /// <summary>
    /// Some extension methods for list to avoid using linq.
    /// </summary>
    public static class ListExtension
    {
        public static T FirstOrDefault<T>(this List<T> list, Func<T, bool> predicate)
        {
            if (list == null) throw new ArgumentNullException("list");
            if (predicate == null) throw new ArgumentNullException("predicate");

            foreach (T element in list)
            {
                if (predicate(element))
                    return element;
            }
            return default;
        }
        private static readonly Random random = new();

        public static void Shuffle<T>(this List<T> list)
        {
            if (list == null) throw new ArgumentNullException("list");
            int count = list.Count;
            while (count > 1)
            {
                count--;
                int k = random.Next(count + 1);
                (list[count], list[k]) = (list[k], list[count]);
            }
        }
    }
}
