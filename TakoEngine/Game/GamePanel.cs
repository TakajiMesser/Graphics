using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TakoEngine.Entities;
using TakoEngine.Inputs;
using TakoEngine.Maps;
using TakoEngine.Outputs;
using TakoEngine.Rendering.Processing;
using TakoEngine.Rendering.Textures;
using TakoEngine.Utilities;
using Timer = System.Timers.Timer;

namespace TakoEngine.Game
{
    public class GamePanel : GLControl
    {
        public const float MAX_ANGLE_Y = (float)Math.PI / 2.0f + 0.1f;
        public const float MIN_ANGLE_Y = -(float)Math.PI / 2.0f + 0.1f;

        public RenderModes RenderMode { get; set; }
        public double Frequency { get; private set; }
        public Resolution Resolution { get; private set; }
        public Resolution PanelSize { get; private set; }
        public bool IsMouseInPanel { get; private set; }
        public bool IsCameraMoving { get; private set; }
        public IEntity SelectedEntity { get; private set; }

        public event EventHandler<CursorEventArgs> ChangeCursorVisibility;
        public event EventHandler<EntitySelectedEventArgs> EntitySelectionChanged;

        private Map _map;

        private EditorGameState _gameState;
        private bool _invalidated = false;
        private Vector3 _currentAngles = new Vector3();
        internal InputState _inputState = new InputState();
        private Point _mouseLocation;
        private Timer _pollTimer = new Timer();

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

        public GamePanel() : base(GraphicsMode.Default, 3, 0, GraphicsContextFlags.ForwardCompatible)
        {
            Resolution = new Resolution(Width, Height);
            PanelSize = new Resolution(Width, Height);

            //Console.WriteLine("GL Version: " + GL.GetString(StringName.Version));

            _pollTimer.Interval = 16.67;
            _pollTimer.Elapsed += PollTimer_Elapsed;
        }

        public void LoadFromMap(string mapPath)
        {
            _map = Map.Load(mapPath);

            if (_gameState != null)
            {
                _gameState.LoadMap(_map, true);
                _gameState.Initialize();
                _pollTimer.Start();
            }
        }

        public void LoadFromModel(string modelPath)
        {
            _map = new Map();

            /*mainActor.TexturesPaths.Add(new TexturePaths()
            {
                DiffuseMapFilePath = FilePathHelper.BRICK_01_D_TEXTURE_PATH,
                NormalMapFilePath = FilePathHelper.BRICK_01_N_NORMAL_PATH,
                SpecularMapFilePath = FilePathHelper.BRICK_01_S_TEXTURE_PATH
            });

            mainActor.TexturesPaths.Add(new TexturePaths()
            {
                DiffuseMapFilePath = FilePathHelper.BRICK_01_D_TEXTURE_PATH,
                NormalMapFilePath = FilePathHelper.BRICK_01_N_NORMAL_PATH,
                SpecularMapFilePath = FilePathHelper.BRICK_01_S_TEXTURE_PATH
            });*/

            var mainActor = new MapActor()
            {
                Name = Path.GetFileNameWithoutExtension(modelPath),
                ModelFilePath = modelPath,
                Position = new Vector3(0.0f, 0.0f, 0.0f),
                Scale = Vector3.One,
                Rotation = Vector3.Zero,
                Orientation = Vector3.Zero,
                HasCollision = true
            };
            _map.Actors.Add(mainActor);

            _map.Camera = new MapCamera()
            {
                AttachedActorName = mainActor.Name,
                Position = new Vector3(5.0f, 5.0f, 5.0f),
                Type = Rendering.Matrices.ProjectionTypes.Perspective,
                FieldOfViewY = UnitConversions.ToRadians(45.0f)
            };

            if (_gameState != null)
            {
                _gameState.LoadMap(_map, false);
                _gameState.Initialize();
                _pollTimer.Start();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            Location = new Point(0, 0);

            _gameState = new EditorGameState(Resolution, PanelSize);

            if (_map != null)
            {
                _gameState.LoadMap(_map, false);
                _gameState.Initialize();
                _pollTimer.Start();
            }
        }

        private void PollTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _pollTimer.Stop();

            // Handle user input, then poll their input for handling on the following frame
            HandleInput();
            PollForInput();

            if (_invalidated)
            {
                Invalidate();
                _invalidated = false;
            }

            _pollTimer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            MakeCurrent();
            OnRenderFrame();
        }

        protected override void OnResize(EventArgs e)
        {
            /*if (Resolution != null)
            {
                Resolution.Width = Width;
                Resolution.Height = Height;
                _gameState?.Resize();
            }*/
            if (PanelSize != null)
            {
                PanelSize.Width = Width;
                PanelSize.Height = Height;
                _gameState?.ResizeWindow();
            }
        }

        protected void OnRenderFrame()
        {
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

            if (SelectedEntity != null)
            {
                _gameState.RenderSelection(SelectedEntity);
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
                _invalidated = true;

                Invoke(new Action(() =>
                {
                    var point = PointToClient(System.Windows.Forms.Cursor.Position);
                    var mouseCoordinates = new Vector2(point.X - Location.X, Height - point.Y - Location.Y);

                    SelectedEntity = _gameState.GetEntityForPoint(mouseCoordinates);
                    EntitySelectionChanged?.Invoke(this, new EntitySelectedEventArgs(SelectedEntity));
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
