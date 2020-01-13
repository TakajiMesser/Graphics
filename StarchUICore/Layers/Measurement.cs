namespace StarchUICore.Layers
{
    public class Measurement
    {
        public Measurement(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; set; }
        public int Height { get; set; }

        public bool NeedsRemeasurement { get; set; }

        public static Measurement Empty => new Measurement(0, 0);
    }
}
