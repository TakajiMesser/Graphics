using OpenTK;

namespace SpiceEngineCore.Inputs
{
    public interface ISelectionTracker
    {
        int GetEntityIDFromSelection(Vector2 coordinates);
    }
}
