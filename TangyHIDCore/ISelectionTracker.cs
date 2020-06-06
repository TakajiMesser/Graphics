using OpenTK;

namespace TangyHIDCore
{
    public interface ISelectionTracker
    {
        int GetEntityIDFromSelection(Vector2 coordinates);
    }
}
