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
        public IEntity SelectedEntity { get; private set; }
        public SelectionTypes SelectionType { get; private set; }
        public bool IsMouseInPanel { get; private set; }
        public bool IsCameraMoving { get; private set; }
        public bool IsSelectionTransforming { get; private set; }

        public event EventHandler<CursorEventArgs> ChangeCursorVisibility;
        public event EventHandler<EntitySelectedEventArgs> EntitySelectionChanged;
        public event EventHandler<TransformSelectedEventArgs> TransformSelectionChanged;

        private Map _map;

        private GameState _gameState;
        private RenderManager _renderManager;

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
                _gameState.LoadMap(_map);
                _gameState.DetachCamera();
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
                _gameState.LoadMap(_map);
                _gameState.Initialize();
                _pollTimer.Start();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            Location = new Point(0, 0);

            _gameState = new GameState(Resolution);
            _renderManager = new RenderManager(Resolution, PanelSize);
            
            if (_map != null)
            {
                _gameState.LoadMap(_map);
                _gameState.DetachCamera();
                _gameState.Initialize();
                _renderManager.Load(_gameState, _map);
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
            if (Resolution != null)
            {
                Resolution.Width = Width;
                Resolution.Height = Height;
                _renderManager?.ResizeResolution();
            }

            if (PanelSize != null)
            {
                PanelSize.Width = Width;
                PanelSize.Height = Height;
                _renderManager?.ResizeWindow();
            }
        }

        protected void OnRenderFrame()
        {
            switch (RenderMode)
            {
                case RenderModes.Wireframe:
                    _renderManager.RenderWireframe(_gameState);
                    _renderManager.RenderEntityIDs(_gameState);
                    break;
                case RenderModes.Diffuse:
                    _renderManager.RenderDiffuseFrame(_gameState);
                    _renderManager.RenderEntityIDs(_gameState);
                    break;
                case RenderModes.Lit:
                    _renderManager.RenderLitFrame(_gameState);
                    _renderManager.RenderEntityIDs(_gameState);
                    break;
                case RenderModes.Full:
                    _renderManager.RenderFullFrame(_gameState);
                    break;
            }

            if (SelectedEntity != null)
            {
                _renderManager.RenderSelection(_gameState.Camera, SelectedEntity);
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

            _gameState.Camera._viewMatrix.LookAt = _gameState.Camera.Position + new Vector3()
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

            _gameState.Camera._viewMatrix.Up = new Vector3()
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
                var right = Vector3.Cross(_gameState.Camera._viewMatrix.Up, _gameState.Camera._viewMatrix.LookAt - _gameState.Camera.Position).Normalized();

                var verticalTranslation = _gameState.Camera._viewMatrix.Up * _gameState._inputState.MouseDelta.Y * 0.02f;
                var horizontalTranslation = right * _gameState._inputState.MouseDelta.X * 0.02f;

                _gameState.Camera.Position -= verticalTranslation + horizontalTranslation;
                _gameState.Camera._viewMatrix.LookAt -= verticalTranslation + horizontalTranslation;
                IsCameraMoving = true;
                IsCursorVisible = false;
                _invalidated = true;
            }
            else if ((_gameState._inputState.IsPressed(new Input(MouseButton.Left)) && IsMouseInPanel)
                    || (_gameState._inputState.IsHeld(new Input(MouseButton.Left)) && IsCameraMoving))
            {
                // Left mouse button allows "moving"
                var translation = (_gameState.Camera._viewMatrix.LookAt - _gameState.Camera.Position) * _gameState._inputState.MouseDelta.Y * 0.02f;
                //var translation = new Vector3(_gameState._camera._viewMatrix.LookAt.X - _gameState._camera.Position.X, _gameState._camera._viewMatrix.LookAt.Y - _gameState._camera.Position.Y, 0.0f) * _gameState._inputState.MouseDelta.Y * 0.02f;
                _gameState.Camera.Position -= translation;

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

                    var id = _renderManager.GetEntityIDFromPoint(mouseCoordinates);

                    switch (id)
                    {
                        case SelectionRenderer.RED_ID:
                            IsSelectionTransforming = true;
                            SelectionType = SelectionTypes.Red;
                            break;

                        case SelectionRenderer.GREEN_ID:
                            IsSelectionTransforming = true;
                            SelectionType = SelectionTypes.Green;
                            break;

                        case SelectionRenderer.BLUE_ID:
                            IsSelectionTransforming = true;
                            SelectionType = SelectionTypes.Blue;
                            break;

                        case SelectionRenderer.CYAN_ID:
                            IsSelectionTransforming = true;
                            SelectionType = SelectionTypes.Cyan;
                            break;

                        case SelectionRenderer.MAGENTA_ID:
                            IsSelectionTransforming = true;
                            SelectionType = SelectionTypes.Magenta;
                            break;

                        case SelectionRenderer.YELLOW_ID:
                            IsSelectionTransforming = true;
                            SelectionType = SelectionTypes.Yellow;
                            break;

                        default:
                            _invalidated = true;
                            SelectedEntity = (id > 0) ? _gameState.GetByID(id) : null;
                            EntitySelectionChanged?.Invoke(this, new EntitySelectedEventArgs(SelectedEntity));
                            break;
                    }
                }));
            }
            else if (_gameState._inputState.IsHeld(new Input(MouseButton.Middle)) && IsSelectionTransforming)
            {
                var position = SelectedEntity.Position;

                // TODO - Can use entity's current rotation to determine position adjustment by that angle, rather than by MouseDelta.Y
                switch (SelectionType)
                {
                    case SelectionTypes.Red:
                        position.X -= _gameState._inputState.MouseDelta.Y * 0.002f;
                        break;
                    case SelectionTypes.Green:
                        position.Y -= _gameState._inputState.MouseDelta.Y * 0.002f;
                        break;
                    case SelectionTypes.Blue:
                        position.Z -= _gameState._inputState.MouseDelta.Y * 0.002f;
                        break;
                    case SelectionTypes.Cyan:
                        position.Y += _gameState._inputState.MouseDelta.X * 0.002f;
                        position.Z -= _gameState._inputState.MouseDelta.Y * 0.002f;
                        break;
                    case SelectionTypes.Magenta:
                        position.Z -= _gameState._inputState.MouseDelta.Y * 0.002f;
                        position.X += _gameState._inputState.MouseDelta.X * 0.002f;
                        break;
                    case SelectionTypes.Yellow:
                        position.X += _gameState._inputState.MouseDelta.X * 0.002f;
                        position.Y -= _gameState._inputState.MouseDelta.Y * 0.002f;
                        break;
                }

                _invalidated = true;
                IsCursorVisible = false;
                SelectedEntity.Position = position;

                Invoke(new Action(() => EntitySelectionChanged?.Invoke(this, new EntitySelectedEventArgs(SelectedEntity))));
                
            }
            else
            {
                IsCameraMoving = false;
                IsSelectionTransforming = false;
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
