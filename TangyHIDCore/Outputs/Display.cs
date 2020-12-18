using OpenTK;
using SpiceEngineCore.Rendering;

namespace TangyHIDCore.Outputs
{
    public class Display
    {
        public string Name { get; set; }

        public Resolution FullscreenResolution { get; set; }
        public Resolution WindowedResolution { get; set; }
        public Vector2 ScreenCenter { get; set; }
        public bool IsFullscreen { get; set; }

        public Resolution CurrentResolution => IsFullscreen
            ? FullscreenResolution
            : WindowedResolution;

        public Display(string name, Resolution resolution, bool isFullscreen = false)
        {
            Name = name;
            WindowedResolution = resolution;
            FullscreenResolution = new Resolution(DisplayDevice.Default.Width, DisplayDevice.Default.Height);
            IsFullscreen = isFullscreen;
        }
    }
}
