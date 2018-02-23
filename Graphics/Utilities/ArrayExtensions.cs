using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphics.Utilities
{
    public static class ArrayExtensions
    {
        public static T[] Initialize<T>(int size, T defaultValue)
        {
            var array = new T[size];

            for (var i = 0; i < size; i++)
            {
                array[i] = defaultValue;
            }

            return array;
        }

        public static void Populate<T>(this T[] source, T value)
        {
            for (var i = 0; i < source.Length; i++)
            {
                source[i] = value;
            }
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
    }
}