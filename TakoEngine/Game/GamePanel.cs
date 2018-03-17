using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TakoEngine.Inputs;
using TakoEngine.Maps;
using TakoEngine.Outputs;
using TakoEngine.Rendering.Processing;
using TakoEngine.Utilities;

namespace TakoEngine.Game
{
    public class GamePanel : GLControl
    {
        public const float MAX_ANGLE_Y = (float)Math.PI / 2.0f + 0.1f;
        public const float MIN_ANGLE_Y = -(float)Math.PI / 2.0f + 0.1f;

        public RenderModes RenderMode { get; set; }
        public double Frequency { get; private set; }
        public Resolution Resolution { get; private set; }
        public EventHandler Resized;
        public bool IsMouseInPanel { get; private set; }
        public bool IsCameraMoving { get; private set; }

        public event EventHandler<CursorEventArgs> ChangeCursorVisibility;
        public event EventHandler<EntitySelectedEventArgs> EntitySelectionChanged;

        private string _mapPath;
        private GameState _gameState;
        private bool _invalidated = false;

        private Vector3 _currentAngles = new Vector3();

        internal InputState _inputState = new InputState();
        private Point _mouseLocation;

        private System.Timers.Timer _timer = new System.Timers.Timer();
        private System.Timers.Timer _fpsTimer = new System.Timers.Timer(1000);
        private List<double> _frequencies = new List<double>();

        private object _cursorLock = new object();
        private bool _isCursorVisible = true;
        public bool IsCursorVisible
        {
            get
            {
                lock (_cursorLock)
                {
                    return _isCursorVisible;
                }
            }
            set
            {
                lock (_cursorLock)
                {
                    if (value != _isCursorVisible)
                    {
                        if (value)
                        {
                            //Cursor.Show();
                            ChangeCursorVisibility?.Invoke(this, new CursorEventArgs(true));
                        }
                        else
                        {
                            //Cursor.Hide();
                            ChangeCursorVisibility?.Invoke(this, new CursorEventArgs(false));
                            //Cursor.Position = new Point(0, 0);
                        }

                        _isCursorVisible = value;
                    }
                }
            }
        }

        public GamePanel(string mapPath) : base(GraphicsMode.Default, 3, 0, GraphicsContextFlags.ForwardCompatible)
        {
            Resolution = new Resolution(Width, Height);

            _mapPath = mapPath;
            //Console.WriteLine("GL Version: " + GL.GetString(StringName.Version));

            _fpsTimer.Elapsed += (s, e) =>
            {
                if (_frequencies.Count > 0)
                {
                    _gameState?.SetFrequency(_frequencies.Average());
                    _frequencies.Clear();
                }
            };
            _fpsTimer.Start();

            _timer.Interval = 16.67;
            _timer.Elapsed += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();

            OnUpdateFrame();

            if (_invalidated)
            {
                Invalidate();
                _invalidated = false;
            }

            _timer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            MakeCurrent();
            OnRenderFrame();
        }

        protected override void OnResize(EventArgs e)
        {
            if (Resolution != null)
            {
                Resolution.Width = Width;
                Resolution.Height = Height;
                _gameState?.Resize();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            Location = new Point(0, 0);

            var loadedMap = Map.Load(_mapPath);

            _gameState = new GameState(loadedMap, Resolution);
            _gameState.Initialize();
            _gameState._camera.DetachFromEntity();

            // Set camera to default position when _horizontalAngle = 0 and _verticalAngle = 0
            _gameState._camera._viewMatrix.Up = Vector3.UnitZ;
            _gameState._camera._viewMatrix.LookAt = _gameState._camera.Position + Vector3.UnitY;

            _timer.Start();
        }

        protected void OnUpdateFrame()
        {
            // Handle game logic, guaranteed to run at a fixed rate, regardless of FPS
            HandleInput();
            //_gameState._inputState.UpdateState(Keyboard.GetState(), Mouse.GetState());
            PollForInput();
        }

        protected void OnRenderFrame()
        {
            _frequencies.Add(Frequency);

            switch (RenderMode)
            {
                case RenderModes.Wireframe:
                    _gameState.RenderWireframe();
                    _gameState.RenderEntityIDs();
                    break;
                case RenderModes.Diffuse:
                    _gameState.RenderDiffuseFrame();
                    _gameState.RenderEntityIDs();
                    break;
                case RenderModes.Lit:
                    _gameState.RenderLitFrame();
                    _gameState.RenderEntityIDs();
                    break;
                case RenderModes.Full:
                    _gameState.RenderFullFrame();
                    break;
            }

            GL.UseProgram(0);
            SwapBuffers();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            IsMouseInPanel = true;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            IsMouseInPanel = false;
            base.OnMouseLeave(e);
        }

        private void CalculateDirection()
        {
            var horizontal = Math.Cos(_currentAngles.Y);
            var vertical = Math.Sin(_currentAngles.Y);

            _gameState._camera._viewMatrix.LookAt = _gameState._camera.Position + new Vector3()
            {
                X = (float)(horizontal * Math.Sin(_currentAngles.X)),
                Y = (float)(horizontal * Math.Cos(_currentAngles.X)),
                Z = -(float)vertical
            };
        }

        private void CalculateUp()
        {
            var yAngle = _currentAngles.Y - (float)Math.PI / 2.0f;

            var horizontal = Math.Cos(yAngle);
            var vertical = Math.Sin(yAngle);

            _gameState._camera._viewMatrix.Up = new Vector3()
            {
                X = (float)(horizontal * Math.Sin(_currentAngles.X)),
                Y = (float)(horizontal * Math.Cos(_currentAngles.X)),
                Z = -(float)vertical
            };
        }

        private void HandleInput()
        {
            if ((_gameState._inputState.IsPressed(new Input(MouseButton.Left)) && _gameState._inputState.IsPressed(new Input(MouseButton.Right)) && IsMouseInPanel)
                    || (_gameState._inputState.IsHeld(new Input(MouseButton.Left)) && _gameState._inputState.IsHeld(new Input(MouseButton.Right)) && IsCameraMoving))
            {
                // Both mouse buttons allow "strafing"
                var right = Vector3.Cross(_gameState._camera._viewMatrix.Up, _gameState._camera._viewMatrix.LookAt - _gameState._camera.Position).Normalized();

                var verticalTranslation = _gameState._camera._viewMatrix.Up * _gameState._inputState.MouseDelta.Y * 0.02f;
                var horizontalTranslation = right * _gameState._inputState.MouseDelta.X * 0.02f;

                _gameState._camera.Position -= verticalTranslation + horizontalTranslation;
                _gameState._camera._viewMatrix.LookAt -= verticalTranslation + horizontalTranslation;
                IsCameraMoving = true;
                IsCursorVisible = false;
                _invalidated = true;
            }
            else if ((_gameState._inputState.IsPressed(new Input(MouseButton.Left)) && IsMouseInPanel)
                    || (_gameState._inputState.IsHeld(new Input(MouseButton.Left)) && IsCameraMoving))
            {
                // Left mouse button allows "moving"
                var translation = (_gameState._camera._viewMatrix.LookAt - _gameState._camera.Position) * _gameState._inputState.MouseDelta.Y * 0.02f;
                //var translation = new Vector3(_gameState._camera._viewMatrix.LookAt.X - _gameState._camera.Position.X, _gameState._camera._viewMatrix.LookAt.Y - _gameState._camera.Position.Y, 0.0f) * _gameState._inputState.MouseDelta.Y * 0.02f;
                _gameState._camera.Position -= translation;

                var currentAngles = _currentAngles;
                var mouseDelta = _gameState._inputState.MouseDelta * 0.001f;

                if (mouseDelta != Vector2.Zero)
                {
                    currentAngles.X += mouseDelta.X;
                    _currentAngles = currentAngles;

                    CalculateDirection();
                }

                CalculateUp();

                IsCameraMoving = true;
                IsCursorVisible = false;
                _invalidated = true;
            }
            else if ((_gameState._inputState.IsPressed(new Input(MouseButton.Right)) && IsMouseInPanel)
                    || (_gameState._inputState.IsHeld(new Input(MouseButton.Right)) && IsCameraMoving))
            {
                // Right mouse button allows "turning"
                var currentAngles = _currentAngles;
                var mouseDelta = _gameState._inputState.MouseDelta * 0.001f;

                if (mouseDelta != Vector2.Zero)
                {
                    currentAngles.X += mouseDelta.X;
                    currentAngles.Y += mouseDelta.Y;
                    currentAngles.Y = currentAngles.Y.Clamp(MIN_ANGLE_Y, MAX_ANGLE_Y);
                    _currentAngles = currentAngles;

                    CalculateDirection();
                    CalculateUp();
                }

                IsCameraMoving = true;
                IsCursorVisible = false;
                _invalidated = true;
            }
            else if (_gameState._inputState.IsPressed(new Input(MouseButton.Middle)) && IsMouseInPanel)
            {
                Invoke(new Action(() =>
                {
                    var point = PointToClient(System.Windows.Forms.Cursor.Position);
                    var mouseCoordinates = new Vector2(point.X - Location.X, Height - point.Y - Location.Y);

                    var entity = _gameState.GetEntityForPoint(mouseCoordinates);
                    EntitySelectionChanged?.Invoke(this, new EntitySelectedEventArgs(entity));
                }));
            }
            else
            {
                IsCameraMoving = false;
                IsCursorVisible = true;
            }
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            _mouseLocation = e.Location;
        }

        private void PollForInput()
        {
            _gameState._inputState.UpdateState(Keyboard.GetState(), Mouse.GetState());
        }
    }
}
