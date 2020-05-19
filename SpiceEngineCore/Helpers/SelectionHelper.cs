using OpenTK;
using OpenTK.Graphics;

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

        public static int GetIDFromColorVector(Vector4 colorVector) => (int)(colorVector.X + colorVector.Y * 256 + colorVector.Z * 256 * 256);
    }
}
