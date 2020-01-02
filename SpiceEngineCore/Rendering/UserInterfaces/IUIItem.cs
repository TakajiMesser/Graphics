using SpiceEngineCore.Rendering.UserInterfaces.Attributes;
using SpiceEngineCore.Rendering.UserInterfaces.Layers;
using SpiceEngineCore.Rendering.UserInterfaces.Views;

namespace SpiceEngineCore.Rendering.UserInterfaces
{
    public interface IUIItem : IRenderable
    {
        IUIItem Parent { get; set; }

        Position Position { get; set; }
        Size Size { get; set; }

        UIBorder Border { get; set; }
        bool IsEnabled { get; set; }
        bool IsVisible { get; set; }

        UIQuadSet Measure();
        void Update();
    }
}
