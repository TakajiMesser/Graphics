using System.Drawing;

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
    public static class OpenTKExtensions
    {
        public static Vector2 ToVector2(this Point point) => new Vector2(point.X, point.Y);
    }
}
