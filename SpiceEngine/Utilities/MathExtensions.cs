using System;

namespace SpiceEngine.Utilities
{
    public static class MathExtensions
    {
        public const float EPSILON = 1E-5f;
        public const float PI = (float)Math.PI;
        public const float HALF_PI = (float)Math.PI / 2.0f;
        public const float TWO_PI = 2.0f * (float)Math.PI;

        public static bool IsSignificant(this int value) => value >= EPSILON || value <= -EPSILON;
        public static bool IsSignificant(this float value) => value >= EPSILON || value <= -EPSILON;

        public static bool IsBetween(this int value, int valueA, int valueB) => (value > valueA && value < valueB) || (value < valueA && value > valueB);
        public static bool IsBetween(this float value, float valueA, float valueB) => (value > valueA && value < valueB) || (value < valueA && value > valueB);

        public static int Clamp(this int value, int minValue, int maxValue)
        {
            if (value > maxValue)
            {
                return maxValue;
            }
            else if (value < minValue)
            {
                return minValue;
            }
            else
            {
                return value;
            }
        }

        public static float Clamp(this float value, float minValue, float maxValue)
        {
            if (value > maxValue)
            {
                return maxValue;
            }
            else if (value < minValue)
            {
                return minValue;
            }
            else
            {
                return value;
            }
        }

        public static int Round(this int value, int min, int max)
        {
            if (value <= min)
            {
                return min;
            }
            else if (value >= max)
            {
                return max;
            }
            else
            {
                return max - value < value - min
                    ? max
                    : min;
            }
        }

        public static float Round(this float value, float min, float max)
        {
            if (value <= min)
            {
                return min;
            }
            else if (value >= max)
            {
                return max;
            }
            else
            {
                return max - value < value - min
                    ? max
                    : min;
            }
        }
    }
}
