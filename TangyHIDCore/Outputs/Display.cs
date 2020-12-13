using SpiceEngineCore.Rendering;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

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
            FullscreenResolution = new Resolution(OpenTK.DisplayDevice.Default.Width, OpenTK.DisplayDevice.Default.Height);
            IsFullscreen = isFullscreen;
        }
    }
}
