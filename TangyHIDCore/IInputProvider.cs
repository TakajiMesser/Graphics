using SpiceEngineCore.Game;
using SpiceEngineCore.Rendering;
using System;
using TangyHIDCore.Inputs;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace TangyHIDCore
{
    public interface IInputProvider : IGameSystem
    {
        InputBinding InputMapping { get; set; }

        bool IsMouseInWindow { get; }
        Vector2? MouseCoordinates { get; }
        Vector2? RelativeCoordinates { get; }
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
