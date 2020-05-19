using OpenTK;
using SpiceEngineCore.Rendering;

namespace SpiceEngineCore.Game.Loading.Builders
{
    public interface IRenderableEditorBuilder
    {
        Vector3 Position { get; set; }

        IRenderable ToRenderable(bool isInEditorMode);
    }
}
