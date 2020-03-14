using OpenTK;

namespace SpiceEngineCore.Inputs
{
    public interface ISelectionTracker
    {
        int GetEntityIDFromPoint(Vector2 point);
    }
}
