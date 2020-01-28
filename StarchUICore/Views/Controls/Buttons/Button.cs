using SpiceEngineCore.Rendering.Vertices;
using StarchUICore.Builders;
using StarchUICore.Themes;

namespace StarchUICore.Views.Controls.Buttons
{
    public class Button : Control
    {
        public Button() { }
        public Button(Vertex3DSet<ViewVertex> vertexSet) : base(vertexSet) { }

        public static Button CreateButton(int x, int y, int width, int height)
        {
            var vertexSet = UIBuilder.Rectangle(width, height, ThemeManager.CurrentTheme.PrimaryBackgroundColor);
            return new Button(vertexSet)
            {
                Position = new Attributes.Position(x, y),
                Size = new Attributes.Size(width, height)
            };
        }
    }
}
