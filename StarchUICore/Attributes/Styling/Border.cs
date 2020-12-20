using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace StarchUICore.Attributes.Styling
{
    public struct Border
    {
        public float Thickness { get; private set; }
        public Color4 Color { get; private set; }
        public float CornerXRadius { get; private set; }
        public float CornerYRadius { get; private set; }

        public Border(float thickness, Color4 color, float cornerXRadius, float cornerYRadius)
        {
            Thickness = thickness;
            Color = color;
            CornerXRadius = cornerXRadius;
            CornerYRadius = cornerYRadius;
        }
    }
}
