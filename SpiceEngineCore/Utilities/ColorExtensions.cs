using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SpiceEngineCore.Utilities
{
    public static class ColorExtensions
    {
        public static Vector3 ToRGB(this Color4 color) => new Vector3(color.R, color.G, color.B);

        public static Vector4 ToVector4(this Color4 color) => new Vector4(color.R, color.G, color.B, color.A);
    }
}
