using SpiceEngineCore.Game;
using SpiceEngineCore.Rendering;
using System;
using TangyHIDCore.Inputs;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

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
