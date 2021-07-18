using System;
using System.Collections.Generic;

namespace SpiceEngineCore.Utilities
{
    public static class ListExtensions
    {
        public static void PadTo<T>(this IList<T> source, T value, int count)
        {
            for (var i = source.Count - 1; i < count; i++)
            {
                source.Add(value);
            }
        }

        public static void Move<T>(this IList<T> source, int oldIndex, int newIndex)
        {
            if (oldIndex < 0 || newIndex < 0) throw new ArgumentOutOfRangeException("Index must be greater than or equal to zero");
            if (oldIndex >= source.Count || newIndex >= source.Count) throw new ArgumentOutOfRangeException("Index must be within item range");

            var item = source[oldIndex];

            source.RemoveAt(oldIndex);
            source.Insert(newIndex, item);
        }
    }
}
