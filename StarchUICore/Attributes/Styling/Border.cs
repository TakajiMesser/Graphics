using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

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
