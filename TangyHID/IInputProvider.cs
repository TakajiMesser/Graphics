using SpiceEngine.GLFWBindings.Inputs;
using SpiceEngineCore.Game;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering;
using System;
using TangyHIDCore.Inputs;

namespace TangyHIDCore
{
    public interface IInputProvider : IGameSystem
    {
        InputMapping InputMapping { get; set; }

        Resolution WindowSize { get; }
        
        Vector2 MouseCoordinates { get; }
        Vector2 MouseDelta { get; }
        int MouseWheelDelta { get; }
        bool IsMouseInWindow { get; }

        event EventHandler<MouseClickEventArgs> MouseDownSelected;
        event EventHandler<MouseClickEventArgs> MouseUpSelected;
        public event EventHandler<EventArgs> EscapePressed;

        void RegisterDevices(IInputTracker inputTracker);

        bool IsDown(string command);
        bool IsUp(string command);
        bool IsPressed(string command);
        bool IsHeld(string command, int nFrames);
        bool IsReleased(string command);

        bool IsDown(Keys key);
        bool IsUp(Keys key);
        bool IsPressed(Keys key);
        bool IsReleased(Keys key);
        bool IsHeld(Keys key, int nFrames);

        bool IsDown(MouseButtons mouseButton);
        bool IsUp(MouseButtons mouseButton);
        bool IsPressed(MouseButtons mouseButton);
        bool IsReleased(MouseButtons mouseButton);
        bool IsHeld(MouseButtons mouseButton, int nFrames);

        bool IsDown(GamePadButtons gamePadButtons);
        bool IsUp(GamePadButtons gamePadButtons);
        bool IsPressed(GamePadButtons gamePadButtons);
        bool IsReleased(GamePadButtons gamePadButtons);
        bool IsHeld(GamePadButtons gamePadButtons, int nFrames);

        //void SwallowInputs(params Input[] inputs);
        void Clear();
    }
}
