using SpiceEngineCore.Geometry.Colors;
using SpiceEngineCore.Geometry.Vectors;

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
