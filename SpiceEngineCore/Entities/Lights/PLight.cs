using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SpiceEngineCore.Entities.Lights
{
    /// <summary>
    /// This struct is used by the Forward Renderer, in a uniform buffer
    /// </summary>
    public struct PLight
    {
        public Vector3 Position { get; private set; }
        public float Radius { get; private set; }
        public Vector3 Color { get; private set; }
        public float Intensity { get; private set; }

        public PLight(Vector3 position, float radius, Vector3 color, float intensity)
        {
            Position = position;
            Radius = radius;
            Color = color;
            Intensity = intensity;
        }
    }
}
