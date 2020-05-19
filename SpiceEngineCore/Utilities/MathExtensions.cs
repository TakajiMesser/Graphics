using System;

namespace SpiceEngineCore.Utilities
{
    public static class MathExtensions
    {
        public const float EPSILON = 1E-5f;
        public const float PI = (float)Math.PI;
        public const float HALF_PI = (float)Math.PI / 2.0f;
        public const float TWO_PI = 2.0f * (float)Math.PI;
        public const float THREE_HALVES_PI = (float)Math.PI * 3.0f / 2.0f;

        public static bool IsSignificant(this int value) => value >= EPSILON || value <= -EPSILON;
        public static bool IsSignificant(this float value) => value >= EPSILON || value <= -EPSILON;

        public static bool IsSignificantDifference(this int value, int comparisonValue) => (value - comparisonValue).IsSignificant();
        public static bool IsSignificantDifference(this float value, float comparisonValue) => (value - comparisonValue).IsSignificant();

        public static bool IsBetween(this int value, int valueA, int valueB) => (value > valueA && value < valueB) || (value < valueA && value > valueB);
        public static bool IsBetween(this float value, float valueA, float valueB) => (value > valueA && value < valueB) || (value < valueA && value > valueB);

        public static bool IsBetweenInclusive(this int value, int valueA, int valueB) => (value >= valueA && value <= valueB) || (value <= valueA && value >= valueB);
        public static bool IsBetweenInclusive(this float value, float valueA, float valueB) => (value >= valueA && value <= valueB) || (value <= valueA && value >= valueB);

        public static bool IsPolarityChange(this double value, double comparisonValue) => (value > 0 && comparisonValue < 0) || (value < 0 && comparisonValue > value);

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

        public static int ClampBottom(this int value, int minValue) => value < minValue ? minValue : value;
        public static float ClampBottom(this float value, float minValue) => value < minValue ? minValue : value;
        public static int ClampTop(this int value, int maxValue) => value > maxValue ? maxValue : value;
        public static float ClampTop(this float value, float maxValue) => value > maxValue ? maxValue : value;

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

        public static int Modulo(this int value, int moduloBy)
        {
            var remainder = value % moduloBy;
            return remainder < 0 ? remainder + moduloBy : remainder;
        }

        public static float Modulo(this float value, float moduloBy) => value - moduloBy * (float)Math.Floor(value / moduloBy);
    }
}
