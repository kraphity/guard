﻿using System.Collections;

namespace Kraphity.Guard
{
    internal static class EnumerableExtensions
    {
        public static bool Any(this IEnumerable enumerable)
        {
            if (enumerable is ICollection c)
                return c.Count > 0;

            return enumerable.GetEnumerator().MoveNext();
        }
    }
}
