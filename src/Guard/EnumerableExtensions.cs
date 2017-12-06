using System.Collections;
using System.Collections.Generic;

namespace Kraphity.Guard
{
    internal static class EnumerableExtensions
    {
        public static int Count(this IEnumerable enumerable)
        {
            if (enumerable is ICollection c)
                return c.Count;

            var count = 0;
            var it = enumerable.GetEnumerator();
            while (it.MoveNext())
            {
                count++;
            }
            return count;
        }

        public static int Count<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable is ICollection<T> c)
                return c.Count;

            return ((IEnumerable)enumerable).Count();
        }
    }
}
