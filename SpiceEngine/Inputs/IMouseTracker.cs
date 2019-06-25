using OpenTK;
using SpiceEngine.Outputs;

namespace SpiceEngine.Inputs
{
    public interface IMouseTracker
    {
        Vector2? MouseCoordinates { get; }
        bool IsMouseInWindow { get; }
        Resolution WindowSize { get; }
    }
}
