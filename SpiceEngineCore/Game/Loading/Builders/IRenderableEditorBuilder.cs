using OpenTK;
using SpiceEngineCore.Rendering;

namespace SpiceEngineCore.Game.Loading.Builders
{
    public interface IRenderableEditorBuilder : IComponentBuilder<IRenderable>
    {
        Vector3 Position { get; set; }

        IRenderable ToComponent(bool isInEditorMode);
    }
}
