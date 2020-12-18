using OpenTK;

namespace SpiceEngineCore.Rendering
{
    public interface IRenderProvider
    {
        IRenderable GetRenderable(int entityID);
        IRenderable GetRenderableOrDefault(int entityID);

        bool HasRenderable(int entityID);

        int GetEntityIDFromSelection(Vector2 coordinates);
    }
}
