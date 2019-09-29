using OpenTK;
using SpiceEngineCore.Outputs;

namespace SpiceEngineCore.Inputs
{
    public interface IMouseTracker
    {
        Vector2? MouseCoordinates { get; }
        bool IsMouseInWindow { get; }
        Resolution WindowSize { get; }
    }
}
