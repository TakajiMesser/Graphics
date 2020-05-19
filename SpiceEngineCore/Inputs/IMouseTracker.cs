using OpenTK;
using SpiceEngineCore.Outputs;

namespace SpiceEngineCore.Inputs
{
    public interface IMouseTracker
    {
        Vector2? MouseCoordinates { get; }
        Vector2? RelativeCoordinates { get; }

        bool IsMouseInWindow { get; }
        Resolution WindowSize { get; }
    }
}
