using System;
using System.Text;

namespace GLWriter.Utilities
{
    public static class StringExtensions
    {
        public static string Capitalized(this string word)
        {
            if (word == null)
            {
                return word;
            }
            else
            {
                var builder = new StringBuilder();

                for (var i = 0; i < word.Length; i++)
                {
                    builder.Append(i == 0 ? char.ToUpper(word[i]) : word[i]);
                }

                return builder.ToString();
            }
        }

        public static bool IsSingular(this string word) => !word.EndsWith("s");

        public static bool IsPlural(this string word) => word.EndsWith("s");

        public static string Singularized(this string word)
        {
            if (word[^1] != 's') throw new ArgumentException(word + " is already singular");

            if (word.EndsWith("ies"))
            {
                return word[0..^3] + "y";
            }
            else
            {
                return word[0..^1];
            }
        }

        public static string Pluralized(this string word)
        {
            if (word[^1] == 's') throw new ArgumentException(word + " is already plural");

            if (word.EndsWith("y"))
            {
                return word[0..^1] + "ies";
            }
            else
            {
                return word + "s";
            }
        }
    }
}
