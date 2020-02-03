namespace StarchUICore.Attributes.Units
{
    public static class Unit
    {
        public static AutoUnits Auto() => new AutoUnits();
        public static PixelUnits Pixels(int value) => new PixelUnits(value);
        public static PercentUnits Percents(float value) => new PercentUnits(value);
    }
}
