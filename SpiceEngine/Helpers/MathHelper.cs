namespace SpiceEngine.Helpers
{
    public static class MathHelper
    {
        public static float Round(float value, float min, float max)
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
