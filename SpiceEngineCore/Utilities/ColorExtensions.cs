using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SpiceEngineCore.Utilities
{
    public static class ColorExtensions
    {
        public static Vector3 ToRGB(this Color4 color) => new Vector3(color.R, color.G, color.B);

        public static Vector4 ToVector4(this Color4 color) => new Vector4(color.R, color.G, color.B, color.A);
    }
}
