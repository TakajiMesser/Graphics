using OpenTK;
using OpenTK.Input;
using SpiceEngine.Outputs;
using System;

namespace SpiceEngine.Inputs
{
    public interface IMouseDelta
    {
        Vector2? MouseCoordinates { get; }
        bool IsMouseInWindow { get; }
        Resolution WindowSize { get; }
    }
}
