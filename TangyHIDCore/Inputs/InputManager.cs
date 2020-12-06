using OpenTK.Input;
using SpiceEngineCore.Game;
using SpiceEngineCore.Geometry.Vectors;
using SpiceEngineCore.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TangyHIDCore.Inputs
{
    public class InputManager : GameSystem, IInputProvider
    {
        public const int DEFAULT_NUMBER_OF_TRACKED_STATES = 2;

        private List<InputState> _inputStates = new List<InputState>();

        public InputBinding InputMapping { get; set; } = new InputBinding();
        public IMouseTracker MouseTracker { get; set; }

        /*public bool IsMouseInWindow
        {
            get
            {
                if (_inputStates.Any())
                {
                    var mousePosition = _inputStates[_inputStates.Count - 1].MousePosition;

                    LogManager.LogToScreen("(" + mousePosition.X + "," + mousePosition.Y + ")");
                }

                return false;
            }
        }*/

        /*public bool IsMouseInWindow => MouseCoordinates.HasValue
            && MouseCoordinates.Value.X.IsBetween(0, WindowSize.Width)
            && MouseCoordinates.Value.Y.IsBetween(0, WindowSize.Height);*/

        /*public Vector2? MouseCoordinates => _inputStates.Any()
            ? _inputStates[_inputStates.Count - 1].MousePosition
            : (Vector2?)null;*/

        public bool IsMouseInWindow => MouseTracker.IsMouseInWindow;
        public Vector2? MouseCoordinates => MouseTracker?.MouseCoordinates;
        public Vector2? RelativeCoordinates => MouseTracker?.RelativeCoordinates;
        public Resolution WindowSize => MouseTracker.WindowSize;

        public int TrackedStates { get; set; } = DEFAULT_NUMBER_OF_TRACKED_STATES;

        public Vector2 MouseDelta => _inputStates.Count >= 2
            ? _inputStates[_inputStates.Count - 1].MousePosition - _inputStates[_inputStates.Count - 2].MousePosition
            : Vector2.Zero;

        public int MouseWheelDelta => _inputStates.Count >= 1
            ? _inputStates.Count >= 2
                ? _inputStates[_inputStates.Count - 2].MouseWheel - _inputStates[_inputStates.Count - 1].MouseWheel
                : _inputStates[_inputStates.Count - 1].MouseWheel
            : 0;

        public event EventHandler<MouseClickEventArgs> MouseDownSelected;
        public event EventHandler<MouseClickEventArgs> MouseUpSelected;
        public event EventHandler<EventArgs> EscapePressed;

        protected override void Update()
        {
            var inputState = new InputState();
            _inputStates.Add(inputState);

            while (_inputStates.Count > TrackedStates)
            {
                _inputStates.RemoveAt(0);
            }

            HandleMouseSelection();

            if (EscapePressed != null && inputState.IsDown(new Input(OpenTK.Input.Key.Escape)))
            {
                EscapePressed.Invoke(this, new EventArgs());
            }
        }

        private void HandleMouseSelection()
        {
            if (MouseDownSelected != null || MouseUpSelected != null)
            {
                if (MouseCoordinates.HasValue && IsMouseInWindow)
                {
                    if (MouseDownSelected != null && IsPressed(new Input(MouseButton.Left)))
                    {
                        MouseDownSelected.Invoke(this, new MouseClickEventArgs(MouseCoordinates.Value));
                    }
                    
                    if (MouseUpSelected != null && IsReleased(new Input(MouseButton.Left)))
                    {
                        MouseUpSelected.Invoke(this, new MouseClickEventArgs(MouseCoordinates.Value));
                    }
                }
            }
        }

        public void Clear() => _inputStates.Clear();

        /*public void HandleInputs(Camera camera, IEnumerable<Actor> actors)
        {
            camera.OnHandleInput(this);

            foreach (var actor in actors)
            {
                actor.OnHandleInput(this, camera);
            }
        }*/

        public bool IsDown(Input input)
        {
            var inputState = _inputStates.LastOrDefault();
            return inputState != null && inputState.IsDown(input);
        }

        public bool IsUp(Input input)
        {
            var inputState = _inputStates.LastOrDefault();
            return inputState != null && inputState.IsUp(input);
        }

        /// <summary>
        /// Determines if this input was triggered this frame but was NOT triggered last frame.
        /// </summary>
        public bool IsPressed(Input input)
        {
            var currentInputState = _inputStates.LastOrDefault();

            if (currentInputState != null)
            {
                if (_inputStates.Count > 1)
                {
                    var previousInputState = _inputStates[_inputStates.Count - 2];
                    return currentInputState.IsDown(input) && !previousInputState.IsDown(input);
                }
                else
                {
                    return currentInputState.IsDown(input);
                }
            }

            return false;
        }

        /// <summary>
        /// Determines if this input was triggered for the requested frame count.
        /// </summary>
        public bool IsHeld(Input input, int nFrames)
        {
            if (nFrames > TrackedStates) throw new ArgumentOutOfRangeException("Not tracking enough frames to determine.");
            
            for (var i = 0; i < nFrames; i++)
            {
                var inputState = _inputStates[_inputStates.Count - 1 - nFrames];
                if (inputState.IsUp(input))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines if this input was triggered last frame, but is not triggered this frame.
        /// </summary>
        public bool IsReleased(Input input)
        {
            var currentInputState = _inputStates.LastOrDefault();

            if (currentInputState != null && _inputStates.Count > 1)
            {
                var previousInputState = _inputStates[_inputStates.Count - 2];
                return currentInputState.IsUp(input) && previousInputState.IsDown(input);
            }

            return false;
        }

        public void SwallowInputs(params Input[] inputs)
        {
            foreach (var input in inputs)
            {
                switch (input.Type)
                {
                    case InputTypes.Key:
                        /*if (_keyState != null)
                        {
                            //_keyState = new KeyboardState();
                        }*/
                        break;
                    case InputTypes.Mouse:
                        break;
                }
            }
        }

        /*private void HandleInput()
        {
            if (_keyState.IsKeyDown(Key.Escape))
            {
                Close();
            }

            if (_previousKeyState != null && _previousKeyState.IsKeyUp(Key.F11) && _keyState.IsKeyDown(Key.F11))
            {
                if (WindowState == WindowState.Normal)
                {
                    WindowState = WindowState.Fullscreen;
                }
                else
                {
                    WindowState = WindowState.Normal;
                }
            }

            if (_previousKeyState != null && _previousKeyState.IsKeyUp(Key.F11) && _keyState.IsKeyDown(Key.F5))
            {
                TakeScreenshot();
            }
        }*/
    }
}
