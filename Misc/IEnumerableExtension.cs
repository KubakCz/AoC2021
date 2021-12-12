using System.Collections.Generic;

namespace AoC2021
{
    public static class IEnumerableExtension
    {
        /// <summary>
        /// Equivalent to python enumerate().
        /// Yields original items with counter.
        /// </summary>
        public static IEnumerable<(int, T)> Enumerate<T>(this IEnumerable<T> collection)
        {
            IEnumerator<T> enumerator = collection.GetEnumerator();
            for (int i = 0; enumerator.MoveNext(); ++i)
                yield return (i, enumerator.Current);
        }
    }
}