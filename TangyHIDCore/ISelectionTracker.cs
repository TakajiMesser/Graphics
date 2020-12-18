using SpiceEngineCore.Geometry.Vectors;

namespace TangyHIDCore
{
    public interface ISelectionTracker
    {
        int GetEntityIDFromSelection(Vector2 coordinates);
    }
}
