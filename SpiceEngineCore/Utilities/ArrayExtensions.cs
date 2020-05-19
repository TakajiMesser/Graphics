using System;

namespace SpiceEngineCore.Utilities
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

        public static T[] Subset<T>(this T[] source, int start) => source.Subset(start, source.Length - start);
        public static T[] Subset<T>(this T[] source, int start, int length)
        {
            if (start < 0 || start >= source.Length) throw new ArgumentOutOfRangeException("Start must be within the bounds of the source array");
            if (start + length > source.Length) throw new ArgumentOutOfRangeException("Length must be within the bounds of the source array");
            if (length <= 0) throw new ArgumentOutOfRangeException("Length must be at least 1");

            var subset = new T[length];

            for (var i = 0; i < length; i++)
            {
                subset[i] = source[start + i];
            }

            return subset;
        }
    }
}