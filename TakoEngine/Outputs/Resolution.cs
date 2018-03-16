namespace TakoEngine.Outputs
{
    public class Resolution
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public float AspectRatio => (float)Width / Height;

        public Resolution(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
