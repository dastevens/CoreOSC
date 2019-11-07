namespace CoreOSC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class Extensions
    {
        public static int FirstIndexAfter<T>(this IEnumerable<T> items, int start, Func<T, bool> predicate)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            if (start >= items.Count())
            {
                throw new ArgumentOutOfRangeException("start");
            }

            var retVal = 0;
            foreach (var item in items)
            {
                if (retVal >= start && predicate(item))
                {
                    return retVal;
                }

                retVal++;
            }

            return -1;
        }

        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            var result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }
}