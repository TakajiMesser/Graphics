using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering;

namespace TangyHIDCore
{
    public interface IMouseTracker
    {
        Vector2? MouseCoordinates { get; }
        Vector2? RelativeCoordinates { get; }

        bool IsMouseInWindow { get; }
        Resolution WindowSize { get; }
    }
}
