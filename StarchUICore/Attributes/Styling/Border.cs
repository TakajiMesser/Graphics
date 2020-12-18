using OpenTK.Graphics;

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
