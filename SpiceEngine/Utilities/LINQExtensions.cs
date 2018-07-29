using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Utilities
{
    public static class LINQExtensions
    {
        public static IEnumerable<T> Generate<T>(params T[] items)
        {
            foreach (var item in items)
            {
                yield return item;
            }
        }

        public static IEnumerable<T> Yield<T>(this T item)
        {
            if (item != null)
            {
                yield return item;
            }
        }

        public static IEnumerable<T> YieldOrDefault<T>(this T item)
        {
            yield return (item != null) ? item : default(T);
        }

        public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> source, int size)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source
                .Select((t, i) => new { Value = t, Index = i })
                .GroupBy(item => item.Index / size, item => item.Value);
        }

        public static IEnumerable<T> DistinctBy<T, U>(this IEnumerable<T> source, Func<T, U> func)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (func == null) throw new ArgumentNullException(nameof(func));

            return source.Any() 
                ? source.GroupBy(func).Select(g => g.First()) 
                : source;
        }

        /// <summary>
        /// Finds the element with the minimum value
        /// </summary>
        /// <typeparam name="T">The type of the items within the enumerable</typeparam>
        /// <typeparam name="U">The type of the value to find the minimum of</typeparam>
        /// <param name="source">The enumerable to search within</param>
        /// <param name="func">The function to apply to each item within the enumerable, in order to find the minimum element</param>
        /// <returns>The element that yielded the minimum value</returns>
        public static T MinElement<T, U>(this IEnumerable<T> source, Func<T, U> func) where U : IComparable
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (func == null) throw new ArgumentNullException(nameof(func));

            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext()) throw new InvalidOperationException("Sequence contains no elements");

                var minElement = sourceIterator.Current;
                var minValue = func(minElement);

                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = func(candidate);

                    if (candidateProjected.CompareTo(minValue) < 0)
                    {
                        minElement = candidate;
                        minValue = candidateProjected;
                    }
                }

                return minElement;
            }
        }

        public static List<T> MinElements<T, U>(this IEnumerable<T> source, Func<T, U> func) where U : IComparable
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (func == null) throw new ArgumentNullException(nameof(func));

            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext()) throw new InvalidOperationException("Sequence contains no elements");

                var minElement = sourceIterator.Current;
                var minValue = func(minElement);
                var minElements = new List<T>() { minElement };

                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = func(candidate);

                    if (candidateProjected.CompareTo(minValue) < 0)
                    {
                        minElement = candidate;
                        minValue = candidateProjected;
                        minElements.Clear();
                        minElements.Add(minElement);
                    }
                    else if (candidateProjected.CompareTo(minValue) == 0)
                    {
                        minElements.Add(candidate);
                    }
                }

                return minElements;
            }
        }

        /// <summary>
        /// Finds the element with the maximum value
        /// </summary>
        /// <typeparam name="T">The type of the items within the enumerable</typeparam>
        /// <typeparam name="U">The type of the value to find the maximum of</typeparam>
        /// <param name="source">The enumerable to search within</param>
        /// <param name="func">The function to apply to each item within the enumerable, in order to find the maximum element</param>
        /// <returns>The element that yielded the maximum value</returns>
        public static T MaxElement<T, U>(this IEnumerable<T> source, Func<T, U> func) where U : IComparable
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (func == null) throw new ArgumentNullException(nameof(func));

            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext()) throw new InvalidOperationException("Sequence contains no elements");

                var maxElement = sourceIterator.Current;
                var maxValue = func(maxElement);

                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = func(candidate);

                    if (candidateProjected.CompareTo(maxValue) > 0)
                    {
                        maxElement = candidate;
                        maxValue = candidateProjected;
                    }
                }

                return maxElement;
            }
        }

        public static IEnumerable<T> MaxElements<T, U>(this IEnumerable<T> source, Func<T, U> func) where U : IComparable
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (func == null) throw new ArgumentNullException(nameof(func));

            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext()) throw new InvalidOperationException("Sequence contains no elements");

                var maxElement = sourceIterator.Current;
                var maxValue = func(maxElement);
                var maxElements = new List<T>() { maxElement };

                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = func(candidate);

                    if (candidateProjected.CompareTo(maxValue) > 0)
                    {
                        maxElement = candidate;
                        maxValue = candidateProjected;
                        maxElements.Clear();
                        maxElements.Add(maxElement);
                    }
                    else if (candidateProjected.CompareTo(maxValue) == 0)
                    {
                        maxElements.Add(candidate);
                    }
                }

                return maxElements;
            }
        }
    }
}