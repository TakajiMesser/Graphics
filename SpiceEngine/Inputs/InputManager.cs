using OpenTK;
using SpiceEngine.Outputs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Inputs
{
    public class InputManager
    {
        public const int DEFAULT_NUMBER_OF_TRACKED_STATES = 2;

        private List<InputState> _inputStates = new List<InputState>();
        private IMouseDelta _mouseDelta;

        public InputManager(IMouseDelta mouseDelta)
        {
            _mouseDelta = mouseDelta;
        }

        public bool IsMouseInWindow => _mouseDelta.IsMouseInWindow;
        public Vector2? MouseCoordinates => _mouseDelta.MouseCoordinates;
        public Resolution WindowSize => _mouseDelta.WindowSize;

        public int TrackedStates { get; set; } = DEFAULT_NUMBER_OF_TRACKED_STATES;

        public Vector2 MouseDelta => _inputStates.Count >= 2
            ? _inputStates[_inputStates.Count - 1].MousePosition - _inputStates[_inputStates.Count - 2].MousePosition
            : Vector2.Zero;

        public int MouseWheelDelta => _inputStates.Count >= 1
            ? _inputStates.Count >= 2
                ? _inputStates[_inputStates.Count - 2].MouseWheel - _inputStates[_inputStates.Count - 1].MouseWheel
                : _inputStates[_inputStates.Count - 1].MouseWheel
            : 0;

        public void Update()
        {
            _inputStates.Add(new InputState());

            while (_inputStates.Count > TrackedStates)
            {
                _inputStates.RemoveAt(0);
            }
        }

        public void Clear()
        {
            _inputStates.Clear();
        }

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

            return inputState != null
                ? inputState.IsDown(input)
                : false;
        }

        public bool IsUp(Input input)
        {
            var inputState = _inputStates.LastOrDefault();

            return inputState != null
                ? inputState.IsUp(input)
                : true;
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
                    case InputType.Key:
                        /*if (_keyState != null)
                        {
                            //_keyState = new KeyboardState();
                        }*/
                        break;
                    case InputType.Mouse:
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
