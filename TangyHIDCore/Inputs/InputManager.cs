using SpiceEngine.GLFWBindings.Inputs;
using SpiceEngineCore.Game;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Utilities;
using System;

namespace TangyHIDCore.Inputs
{
    public class InputManager : GameSystem, IInputProvider, IInputStateProvider
    {
        private KeyDevice _keyboard;
        private MouseDevice _mouse;
        private GamePadDevice _gamePad;

        private KeyState[] _keyStates;
        private MouseState[] _mouseStates;
        private GamePadState[] _gamePadStates;

        private int _stateIndex = 0;

        public InputManager(int nTrackedStates = 2)
        {
            TrackedStates = nTrackedStates;

            _keyStates = ArrayExtensions.Initialize<KeyState>(TrackedStates + 1);
            _mouseStates = ArrayExtensions.Initialize<MouseState>(TrackedStates + 1);
            _gamePadStates = ArrayExtensions.Initialize<GamePadState>(TrackedStates + 1);
        }

        public int TrackedStates { get; }

        public InputMapping InputMapping { get; set; } = InputMapping.Default();
        public IMouseTracker MouseTracker { get; set; }

        public Resolution WindowSize => MouseTracker.WindowSize;

        public bool IsMouseInWindow => _mouseStates[_stateIndex].IsInWindow;
        public Vector2 MouseCoordinates => _mouseStates[_stateIndex].Position;

        public Vector2 MouseDelta => _mouseStates[_stateIndex].Position - _mouseStates[_stateIndex > 0 ? _stateIndex - 1 : TrackedStates - 1].Position;
        public int MouseWheelDelta => _mouseStates[_stateIndex > 0 ? _stateIndex - 1 : TrackedStates - 1].Wheel - _mouseStates[_stateIndex].Wheel;

        public event EventHandler<MouseClickEventArgs> MouseDownSelected;
        public event EventHandler<MouseClickEventArgs> MouseUpSelected;
        public event EventHandler<EventArgs> EscapePressed;

        protected override void Update()
        {
            _keyStates[_stateIndex] = _keyboard.GetState();
            _mouseStates[_stateIndex] = _mouse.GetState();
            _gamePadStates[_stateIndex] = _gamePad.GetState();

            _stateIndex = (_stateIndex + 1) % (TrackedStates + 1);

            HandleMouseSelection();

            if (EscapePressed != null && _keyStates[_stateIndex].IsDown(Keys.Escape))
            {
                EscapePressed.Invoke(this, new EventArgs());
            }
        }

        public void Clear()
        {
            _keyStates = ArrayExtensions.Initialize<KeyState>(TrackedStates + 1);
            _mouseStates = ArrayExtensions.Initialize<MouseState>(TrackedStates + 1);
            _gamePadStates = ArrayExtensions.Initialize<GamePadState>(TrackedStates + 1);
        }

        public void RegisterDevices(IInputTracker inputTracker)
        {
            _keyboard = new KeyDevice(this, inputTracker);
            _mouse = new MouseDevice(this, inputTracker);
            _gamePad = new GamePadDevice(this, inputTracker);
        }

        public bool IsDown(string command)
        {
            foreach (var binding in InputMapping.GetBindings(command))
            {
                var inputState = GetCurrentFrameState(binding.DeviceType);

                if (inputState != null)
                {
                    return inputState.IsDown(binding);
                }
            }

            return false;
        }

        public bool IsUp(string command) => !IsDown(command);

        /// <summary>
        /// Determines if this input was triggered this frame but was NOT triggered last frame.
        /// </summary>
        public bool IsPressed(string command)
        {
            foreach (var binding in InputMapping.GetBindings(command))
            {
                var currentState = GetCurrentFrameState(binding.DeviceType);
                var previousState = GetPreviousFrameState(binding.DeviceType);

                if (currentState != null && previousState != null)
                {
                    return currentState.IsDown(binding) && !previousState.IsDown(binding);
                }
            }

            return false;
        }

        /// <summary>
        /// Determines if this input was triggered last frame, but is not triggered this frame.
        /// </summary>
        public bool IsReleased(string command)
        {
            foreach (var binding in InputMapping.GetBindings(command))
            {
                var currentState = GetCurrentFrameState(binding.DeviceType);
                var previousState = GetPreviousFrameState(binding.DeviceType);

                if (currentState != null && previousState != null)
                {
                    return !currentState.IsDown(binding) && previousState.IsDown(binding);
                }
            }

            return false;
        }

        /// <summary>
        /// Determines if this input was triggered for the requested frame count.
        /// </summary>
        public bool IsHeld(string command, int nFrames)
        {
            if (nFrames > TrackedStates) throw new ArgumentOutOfRangeException("Not tracking enough frames to determine.");

            foreach (var binding in InputMapping.GetBindings(command))
            {
                for (var i = 0; i < nFrames; i++)
                {
                    var index = TrackedStates - 1 - nFrames;
                    var state = GetFrameState(binding.DeviceType, index);

                    if (state == null || !state.IsDown(binding))
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        public bool IsDown(Keys key) => _keyStates[_stateIndex].IsDown(key);
        public bool IsUp(Keys key) => !IsDown(key);
        public bool IsPressed(Keys key) => _keyStates[_stateIndex].IsDown(key) && !_keyStates[_stateIndex > 0 ? _stateIndex - 1 : TrackedStates - 1].IsDown(key);
        public bool IsReleased(Keys key) => !_keyStates[_stateIndex].IsDown(key) && _keyStates[_stateIndex > 0 ? _stateIndex - 1 : TrackedStates - 1].IsDown(key);
        public bool IsHeld(Keys key, int nFrames)
        {
            if (nFrames > TrackedStates) throw new ArgumentOutOfRangeException("Not tracking enough frames to determine.");

            for (var i = 0; i < nFrames; i++)
            {
                var index = TrackedStates - 1 - nFrames;

                if (!_keyStates[index].IsDown(key))
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsDown(MouseButtons mouseButton) => _mouseStates[_stateIndex].IsDown(mouseButton);
        public bool IsUp(MouseButtons mouseButton) => !IsDown(mouseButton);
        public bool IsPressed(MouseButtons mouseButton) => _mouseStates[_stateIndex].IsDown(mouseButton) && !_mouseStates[_stateIndex > 0 ? _stateIndex - 1 : TrackedStates - 1].IsDown(mouseButton);
        public bool IsReleased(MouseButtons mouseButton) => !_mouseStates[_stateIndex].IsDown(mouseButton) && _mouseStates[_stateIndex > 0 ? _stateIndex - 1 : TrackedStates - 1].IsDown(mouseButton);
        public bool IsHeld(MouseButtons mouseButton, int nFrames)
        {
            if (nFrames > TrackedStates) throw new ArgumentOutOfRangeException("Not tracking enough frames to determine.");

            for (var i = 0; i < nFrames; i++)
            {
                var index = TrackedStates - 1 - nFrames;

                if (!_mouseStates[index].IsDown(mouseButton))
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsDown(GamePadButtons gamePadButton) => _gamePadStates[_stateIndex].IsDown(gamePadButton);
        public bool IsUp(GamePadButtons gamePadButton) => !IsDown(gamePadButton);
        public bool IsPressed(GamePadButtons gamePadButton) => _gamePadStates[_stateIndex].IsDown(gamePadButton) && !_gamePadStates[_stateIndex > 0 ? _stateIndex - 1 : TrackedStates - 1].IsDown(gamePadButton);
        public bool IsReleased(GamePadButtons gamePadButton) => !_gamePadStates[_stateIndex].IsDown(gamePadButton) && _gamePadStates[_stateIndex > 0 ? _stateIndex - 1 : TrackedStates - 1].IsDown(gamePadButton);
        public bool IsHeld(GamePadButtons gamePadButton, int nFrames)
        {
            if (nFrames > TrackedStates) throw new ArgumentOutOfRangeException("Not tracking enough frames to determine.");

            for (var i = 0; i < nFrames; i++)
            {
                var index = TrackedStates - 1 - nFrames;

                if (!_gamePadStates[index].IsDown(gamePadButton))
                {
                    return false;
                }
            }

            return true;
        }

        public IInputState GetNextAvailableState(DeviceTypes deviceType) => GetFrameState(deviceType, (_stateIndex + 1) % (TrackedStates + 1));

        private IInputState GetCurrentFrameState(DeviceTypes deviceType) => GetFrameState(deviceType, _stateIndex);

        private IInputState GetPreviousFrameState(DeviceTypes deviceType) => GetFrameState(deviceType, _stateIndex > 0 ? _stateIndex - 1 : TrackedStates - 1);

        private IInputState GetFrameState(DeviceTypes deviceType, int index) => deviceType switch
        {
            DeviceTypes.Key => _keyStates[index],
            DeviceTypes.Mouse => _mouseStates[index],
            DeviceTypes.GamePad => _gamePadStates[index],
            DeviceTypes.None => null,
            _ => throw new ArgumentException("Could not handle device type " + deviceType)
        };

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

        private void HandleMouseSelection()
        {
            if (MouseDownSelected != null || MouseUpSelected != null)
            {
                if (IsMouseInWindow)
                {
                    if (MouseDownSelected != null && IsPressed(MouseButtons.Left))
                    {
                        MouseDownSelected.Invoke(this, new MouseClickEventArgs(MouseCoordinates));
                    }
                    
                    if (MouseUpSelected != null && IsReleased(MouseButtons.Left))
                    {
                        MouseUpSelected.Invoke(this, new MouseClickEventArgs(MouseCoordinates));
                    }
                }
            }
        }

        /*public void HandleInputs(Camera camera, IEnumerable<Actor> actors)
        {
            camera.OnHandleInput(this);

            foreach (var actor in actors)
            {
                actor.OnHandleInput(this, camera);
            }
        }*/

        /*public void SwallowInputs(params Input[] inputs)
        {
            foreach (var input in inputs)
            {
                switch (input.Type)
                {
                    case InputTypes.Key:
                        /*if (_keyState != null)
                        {
                            //_keyState = new KeyboardState();
                        }*
                        break;
                    case InputTypes.Mouse:
                        break;
                }
            }
        }*/

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
