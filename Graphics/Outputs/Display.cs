using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Outputs
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
