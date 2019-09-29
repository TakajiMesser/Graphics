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
    }
}
