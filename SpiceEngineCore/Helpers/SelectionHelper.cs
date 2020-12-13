using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SpiceEngineCore.Helpers
{
    public static class SelectionHelper
    {
        public static Color4 GetColorFromID(int id) => new Color4(
            ((id & 0x000000FF) >> 0) / 255.0f,
            ((id & 0x0000FF00) >> 8) / 255.0f,
            ((id & 0x00FF0000) >> 16) / 255.0f,
            1.0f);

        public static int GetIDFromColorVector(Vector4 colorVector) => (int)(colorVector.X + colorVector.Y * 256 + colorVector.Z * 256 * 256);
    }
}
