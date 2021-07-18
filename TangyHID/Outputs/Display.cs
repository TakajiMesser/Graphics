using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering;

namespace TangyHIDCore.Outputs
{
    public class Display
    {
        public Resolution Resolution { get; set; }
        public Resolution Window { get; set; }

        public Vector2 ScreenCenter { get; set; }
        public bool IsFullscreen { get; set; }

        public Display(int width, int height, bool isFullscreen = false)
        {
            Resolution = new Resolution(width, height);
            Window = new Resolution(width, height);
            //Screen = new Resolution(DisplayDevice.Default.Width, DisplayDevice.Default.Height);
            IsFullscreen = isFullscreen;
        }
    }
}
