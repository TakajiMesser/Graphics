using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SpiceEngineCore.Helpers
{
    public static class SelectionHelper
    {
        public static Color4 GetColorFromID(int id) => new Color4()
        {
            R = ((id & 0x000000FF) >> 0) / 255.0f,
            G = ((id & 0x0000FF00) >> 8) / 255.0f,
            B = ((id & 0x00FF0000) >> 16) / 255.0f,
            A = 1.0f
        };

        public static int GetIDFromColor(Color4 color) => (int)(color.R + color.B * 256 + color.B * 256 * 256);
    }
}
