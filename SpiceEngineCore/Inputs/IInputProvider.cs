using OpenTK;
using SpiceEngineCore.Game;
using SpiceEngineCore.Outputs;
using System;

namespace SpiceEngineCore.Inputs
{
    public interface IInputProvider : ITick
    {
        InputBinding InputMapping { get; set; }

        bool IsMouseInWindow { get; }
        Vector2? MouseCoordinates { get; }
        Resolution WindowSize { get; }

        Vector2 MouseDelta { get; }
        int MouseWheelDelta { get; }

        event EventHandler<MouseClickEventArgs> MouseDownSelected;
        event EventHandler<MouseClickEventArgs> MouseUpSelected;

        bool IsDown(Input input);
        bool IsUp(Input input);

        /// <summary>
        /// Determines if this input was triggered this frame but was NOT triggered last frame.
        /// </summary>
        bool IsPressed(Input input);

        /// <summary>
        /// Determines if this input was triggered for the requested frame count.
        /// </summary>
        bool IsHeld(Input input, int nFrames);

        /// <summary>
        /// Determines if this input was triggered last frame, but is not triggered this frame.
        /// </summary>
        bool IsReleased(Input input);

        void SwallowInputs(params Input[] inputs);
        void Clear();
    }
}
