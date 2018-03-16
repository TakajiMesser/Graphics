namespace TakoEngine.Utilities
{
    public static class MathExtensions
    {
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
    }
}
