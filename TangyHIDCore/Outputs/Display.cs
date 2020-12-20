using OpenTK;
using SpiceEngineCore.Rendering;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

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
