using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.IO;
using TakoEngine;
using TakoEngine.Meshes;
using TakoEngine.GameObjects;
using System.Runtime.InteropServices;
using OpenTK.Input;
using TakoEngine.Physics.Collision;
using TakoEngine.Helpers;
using TakoEngine.Maps;
using TakoEngine.Rendering.Shaders;
using TakoEngine.Scripting.BehaviorTrees;
using TakoEngine.Rendering.PostProcessing;
using TakoEngine.Outputs;
using System.Drawing;
using System.Drawing.Imaging;
using System.Timers;
using System.Windows.Threading;
using System.Windows.Forms;
using TakoEngine.Inputs;
using TakoEngine.Rendering.Processing;
using TakoEngine.Rendering.Textures;
using TakoEngine.Lighting;

namespace TakoEngine.GameObjects
{
    public class GamePanel : OpenTK.GLControl
    {
        private string _mapPath;
        private GameState _gameState;

        private float _horizontalAngle = 0.0f;
        private float _verticalAngle = 0.0f;

        internal InputState _inputState = new InputState();

        private System.Timers.Timer _timer = new System.Timers.Timer();
        private System.Timers.Timer _fpsTimer = new System.Timers.Timer(1000);
        private List<double> _frequencies = new List<double>();

        public double Frequency { get; private set; }
        public Resolution Resolution { get; private set; }
        public EventHandler Resized;
        public bool IsMouseInPanel { get; private set; }
        public bool IsCameraMoving { get; private set; }

        private bool _isCursorVisible = true;
        public bool IsCursorVisible
        {
            get => _isCursorVisible;
            set
            {
                if (value != _isCursorVisible)
                {
                    if (value)
                    {
                        Cursor.Show();
                    }
                    else
                    {
                        Cursor.Hide();
                    }

                    _isCursorVisible = value;
                }
            }
        }

        public GamePanel(string mapPath) : base(GraphicsMode.Default, 3, 0, GraphicsContextFlags.ForwardCompatible)
        {
            Resolution = new Resolution(Width, Height);
            //Resolution = new Resolution(1000, 1000);

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
            OnUpdateFrame();
            Invalidate();
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
            //WindowState = WindowState.Maximized;
            //Size = new System.Drawing.Size(1280, 720);

            var loadedMap = Map.Load(_mapPath);

            _gameState = new GameState(loadedMap, Resolution);
            _gameState.Initialize();
            _gameState._camera.DetachFromGameObject();
            _gameState._camera._viewMatrix.Up = Vector3.UnitZ;
            _gameState._camera._viewMatrix.LookAt = _gameState._camera.Position + Vector3.UnitY;

            _timer.Start();
        }

        //protected override void OnMouseEnter(EventArgs e) => CursorVisible = false;

        //protected override void OnMouseLeave(EventArgs e) => CursorVisible = true;

        protected void OnUpdateFrame()
        {
            // Handle game logic, guaranteed to run at a fixed rate, regardless of FPS
            HandleInput();
            //_gameState._camera.OnUpdateFrame();
            //_gameState._inputState.UpdateState(Keyboard.GetState(), Mouse.GetState());
            PollForInput();
        }

        protected void OnRenderFrame()
        {
            _frequencies.Add(Frequency);
            //_gameState.RenderFrame();
            _gameState.RenderWireframe();

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
            }
            else if ((_gameState._inputState.IsPressed(new Input(MouseButton.Left)) && IsMouseInPanel)
                    || (_gameState._inputState.IsHeld(new Input(MouseButton.Left)) && IsCameraMoving))
            {
                // Left mouse button allows "moving"
                _horizontalAngle -= _gameState._inputState.MouseDelta.X * 0.0025f;
                var direction = new Vector3()
                {
                    X = (float)Math.Cos(_verticalAngle) * (float)Math.Sin(_horizontalAngle),
                    Y = (float)Math.Sin(_verticalAngle),
                    Z = (float)Math.Cos(_verticalAngle) * (float)Math.Cos(_horizontalAngle)
                };

                var right = new Vector3()
                {
                    X = (float)Math.Sin(_horizontalAngle - Math.PI / 2.0),
                    Y = 0.0f,
                    Z = (float)Math.Cos(_horizontalAngle - Math.PI / 2.0)
                };

                var up = Vector3.Cross(right, direction);

                _gameState._camera._viewMatrix.LookAt = _gameState._camera.Position - direction;
                _gameState._camera._viewMatrix.Up = up;

                var translation = (_gameState._camera._viewMatrix.LookAt - _gameState._camera.Position) * _gameState._inputState.MouseDelta.Y * 0.02f;
                _gameState._camera.Position -= translation;
                _gameState._camera._viewMatrix.LookAt -= translation;
                IsCameraMoving = true;
                IsCursorVisible = false;
            }
            else if ((_gameState._inputState.IsPressed(new Input(MouseButton.Right)) && IsMouseInPanel)
                    || (_gameState._inputState.IsHeld(new Input(MouseButton.Right)) && IsCameraMoving))
            {
                // Right mouse button allows "turning"
                _horizontalAngle -= _gameState._inputState.MouseDelta.X * 0.005f;
                _verticalAngle += _gameState._inputState.MouseDelta.Y * 0.005f;

                var direction = new Vector3()
                {
                    X = (float)Math.Cos(_verticalAngle) * (float)Math.Sin(_horizontalAngle),
                    Y = (float)Math.Sin(_verticalAngle),
                    Z = (float)Math.Cos(_verticalAngle) * (float)Math.Cos(_horizontalAngle)
                };

                var right = new Vector3()
                {
                    X = (float)Math.Sin(_horizontalAngle - Math.PI / 2.0),
                    Y = 0.0f,
                    Z = (float)Math.Cos(_horizontalAngle - Math.PI / 2.0)
                };

                var up = Vector3.Cross(right, direction);

                _gameState._camera._viewMatrix.LookAt = _gameState._camera.Position - direction;
                _gameState._camera._viewMatrix.Up = up;
                IsCameraMoving = true;
                IsCursorVisible = false;
            }
            else
            {
                IsCameraMoving = false;
                IsCursorVisible = true;
            }
        }

        private Point _mouseLocation;

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            _mouseLocation = e.Location;
        }

        private void PollForInput()
        {
            //_previousKeyState = _keyState;
            //_previousMouseState = _mouseState;
            //_previousMouse = _mouse;

            //_keyState = Keyboard.GetState();
            //_mouseState = Mouse.GetState();
            //_gameState._inputState.UpdateState(_mouseLocation);
            _gameState._inputState.UpdateState(Keyboard.GetState(), Mouse.GetState());
            //_mouse = Mouse;
        }
    }
}
