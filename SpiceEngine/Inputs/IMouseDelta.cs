using OpenTK;
using SpiceEngine.Outputs;

namespace SpiceEngine.Inputs
{
    public interface IMouseDelta
    {
        Vector2? MouseCoordinates { get; }
        bool IsMouseInWindow { get; }
        Resolution WindowSize { get; }
    }
}
