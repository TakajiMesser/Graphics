using OpenTK;
using OpenTK.Graphics;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SpiceEngine.Utilities
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
    }
}
