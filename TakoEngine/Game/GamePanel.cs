﻿using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TakoEngine.Entities;
using TakoEngine.Entities.Cameras;
using TakoEngine.Entities.Lights;
using TakoEngine.Inputs;
using TakoEngine.Maps;
using TakoEngine.Outputs;
using TakoEngine.Rendering.Processing;
using TakoEngine.Utilities;
using Brush = TakoEngine.Entities.Brush;
using Timer = System.Timers.Timer;

namespace TakoEngine.Game
{
    public enum ViewTypes
    {
        X,
        Y,
        Z,
        Perspective
    }

    public enum Tools
    {
        Selector,
        Volume,
        Brush,
        Mesh,
        Texture
    }

    public class GamePanel : GLControl
    {
        public const float MAX_ANGLE_Y = (float)Math.PI / 2.0f + 0.1f;
        public const float MIN_ANGLE_Y = -(float)Math.PI / 2.0f + 0.1f;

        public Resolution Resolution { get; private set; }
        public Resolution PanelSize { get; private set; }
        public double Frequency { get; private set; }
        public ViewTypes ViewType { get; set; }
        public RenderModes RenderMode { get; set; }
        public TransformModes TransformMode { get; set; }
        public List<IEntity> SelectedEntities { get; } = new List<IEntity>();
        public SelectionTypes SelectionType { get; private set; }
        public Tools SelectedTool
        {
            get => _selectedTool;
            set
            {
                _selectedTool = value;

                switch (_selectedTool)
                {
                    case Tools.Volume:
                        _toolVolume = Volume.RectangularPrism(Vector3.Zero, 10.0f, 10.0f, 10.0f, new Vector4(0.0f, 0.0f, 0.5f, 0.2f));
                        _gameState?.EntityManager.AddEntity(_toolVolume);
                        break;
                    default:
                        if (_toolVolume != null)
                        {
                            _gameState?.EntityManager.RemoveEntityByID(_toolVolume.ID);
                            _toolVolume = null;
                        }
                        break;
                }

                Invalidate();
            }
        }

        public bool IsMouseInPanel { get; private set; }
        public bool IsCameraMoving { get; private set; }
        public bool IsLoaded { get; private set; }
        public bool IsDragging { get; private set; }
        public bool RenderGrid
        {
            get => _renderManager.RenderGrid;
            set
            {
                _renderManager.RenderGrid = value;
                Invalidate();
            }
        }

        public event EventHandler<PanelLoadedEventArgs> PanelLoaded;
        public event EventHandler<CursorEventArgs> ChangeCursorVisibility;
        public event EventHandler<EntitiesEventArgs> EntitySelectionChanged;
        //public event EventHandler<TransformSelectedEventArgs> TransformSelectionChanged;
        //public event EventHandler<TransformModeEventArgs> TransformModeChanged;

        private Map _map;
        private GameState _gameState;
        private RenderManager _renderManager;

        private bool _invalidated = false;
        private Vector3 _currentAngles = new Vector3();
        private Point _currentMouseLocation;
        private Point _startMouseLocation;
        private Timer _pollTimer = new Timer();

        private Tools _selectedTool = Tools.Brush;
        private Volume _toolVolume;

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
                        ChangeCursorVisibility?.Invoke(this, new CursorEventArgs(value));
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

        public void CenterView()
        {
            if (_gameState != null && SelectedEntities.Count > 0)
            {
                switch (ViewType)
                {
                    case ViewTypes.Perspective:
                        break;
                    case ViewTypes.X:
                        var translationX = new Vector3()
                        {
                            X = 0.0f,
                            Y = SelectedEntities.Average(e => e.Position.Y) - _gameState.Camera.Position.Y,
                            Z = SelectedEntities.Average(e => e.Position.Z) - _gameState.Camera.Position.Z
                        };

                        _gameState.Camera.Position += translationX;
                        _gameState.Camera._viewMatrix.LookAt += translationX;
                        break;
                    case ViewTypes.Y:
                        var translationY = new Vector3()
                        {
                            X = SelectedEntities.Average(e => e.Position.X) - _gameState.Camera.Position.X,
                            Y = 0.0f,
                            Z = SelectedEntities.Average(e => e.Position.Z) - _gameState.Camera.Position.Z
                        };

                        _gameState.Camera.Position += translationY;
                        _gameState.Camera._viewMatrix.LookAt += translationY;
                        break;
                    case ViewTypes.Z:
                        var translationZ = new Vector3()
                        {
                            X = SelectedEntities.Average(e => e.Position.X) - _gameState.Camera.Position.X,
                            Y = SelectedEntities.Average(e => e.Position.Y) - _gameState.Camera.Position.Y,
                            Z = 0.0f
                        };

                        _gameState.Camera.Position += translationZ;
                        _gameState.Camera._viewMatrix.LookAt += translationZ;
                        break;
                }

                RenderFrame();
            }
        }

        public void LoadFromModel(string modelPath)
        {
            /*_map = new Map();

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
            }*/
        }

        protected override void OnLoad(EventArgs e)
        {
            Location = new Point(0, 0);
            _gameState = new GameState(Resolution);
            _renderManager = new RenderManager(Resolution, PanelSize);

            if (_map != null)
            {
                LoadGameState();
            }
        }

        /*public void LoadGameState(GameState gameState, Map map)
        {
            _gameState = gameState;
            _map = map;

            _renderManager = new RenderManager(Resolution, PanelSize);
            _renderManager.Load(gameState, map);
            //_pollTimer.Start();
        }*/

        public void LoadFromMap(string filePath)
        {
            _map = Map.Load(filePath);

            /*using (var window = new NativeWindow())
            {
                var glContext = new GraphicsContext(GraphicsMode.Default, window.WindowInfo, 3, 0, GraphicsContextFlags.ForwardCompatible);
                glContext.MakeCurrent(window.WindowInfo);
                (glContext as IGraphicsContextInternal).LoadAll();
            }*/

            if (_gameState != null)
            {
                LoadGameState();
            }
            //_gameState.LoadMap(_map);
            //_gameState.Camera.DetachFromEntity();
            //_gameState.Initialize();
            //LoadGameState();
        }

        private void LoadGameState()
        {
            switch (ViewType)
            {
                case ViewTypes.X:
                    _map.Camera = new MapCamera()
                    {
                        Type = Rendering.Matrices.ProjectionTypes.Orthographic,
                        Position = Vector3.UnitX * -100.0f,//new Vector3(_map.Boundaries.Min.X - 10.0f, 0.0f, 0.0f),
                        StartingWidth = 20.0f,
                        ZNear = -1000.0f,
                        ZFar = 1000.0f
                    };
                    break;
                case ViewTypes.Y:
                    _map.Camera = new MapCamera()
                    {
                        Type = Rendering.Matrices.ProjectionTypes.Orthographic,
                        Position = Vector3.UnitY * -100.0f,//new Vector3(0.0f, _map.Boundaries.Min.Y - 10.0f, 0.0f),
                        StartingWidth = 20.0f,
                        ZNear = -1000.0f,
                        ZFar = 1000.0f
                    };
                    break;
                case ViewTypes.Z:
                    _map.Camera = new MapCamera()
                    {
                        Type = Rendering.Matrices.ProjectionTypes.Orthographic,
                        Position = Vector3.UnitZ * 100.0f,//new Vector3(0.0f, 0.0f, _map.Boundaries.Max.Z + 10.0f),
                        StartingWidth = 20.0f,
                        ZNear = -1000.0f,
                        ZFar = 1000.0f
                    };
                    break;
            }

            //Invoke((MethodInvoker)delegate { });

            _gameState.LoadMap(_map);

            _gameState.Camera.DetachFromEntity();
            _gameState.Initialize();

            _renderManager.Load(_map);
            //_pollTimer.Start();

            switch (ViewType)
            {
                case ViewTypes.Perspective:
                    _currentAngles = new Vector3(0.0f, 0.0f, 0.0f);
                    _gameState.Camera._viewMatrix.Up = Vector3.UnitZ;
                    _gameState.Camera._viewMatrix.LookAt = _gameState.Camera.Position + Vector3.UnitY;
                    break;
                case ViewTypes.X:
                    _currentAngles = new Vector3(90.0f, 0.0f, 0.0f);
                    _gameState.Camera._viewMatrix.Up = Vector3.UnitZ;
                    _gameState.Camera._viewMatrix.LookAt = _gameState.Camera.Position + Vector3.UnitX;
                    _renderManager.RotateGrid(0.0f, (float)Math.PI / 2.0f, 0.0f);
                    break;
                case ViewTypes.Y:
                    _currentAngles = new Vector3(0.0f, 0.0f, 0.0f);
                    _gameState.Camera._viewMatrix.Up = Vector3.UnitZ;
                    _gameState.Camera._viewMatrix.LookAt = _gameState.Camera.Position + Vector3.UnitY;
                    _renderManager.RotateGrid(0.0f, 0.0f, (float)Math.PI / 2.0f);
                    break;
                case ViewTypes.Z:
                    _currentAngles = new Vector3(0.0f, 90.0f, 0.0f);
                    _gameState.Camera._viewMatrix.Up = Vector3.UnitY;
                    _gameState.Camera._viewMatrix.LookAt = _gameState.Camera.Position - Vector3.UnitZ;
                    break;
            }

            IsLoaded = true;
            PanelLoaded?.Invoke(this, new PanelLoadedEventArgs());
        }

        public void SelectEntity(IEntity entity)
        {
            if (entity != null)
            {
                SelectedEntities.Add(entity);
            }
            else
            {
                SelectedEntities.Clear();
            }
            //SelectedEntity = (entity != null) ? _gameState.GetEntityByID(entity.ID) : null;
            RenderFrame();
        }

        public void SelectEntities(IEnumerable<IEntity> entities)
        {
            SelectedEntities.Clear();
            SelectedEntities.AddRange(entities);
            RenderFrame();
        }

        public void UpdateEntities(IEnumerable<IEntity> entities)
        {
            foreach (var entity in entities)
            {
                var selectedEntity = _gameState.EntityManager.GetEntityByID(entity.ID);

                if (selectedEntity != null)
                {
                    selectedEntity.Position = entity.Position;

                    switch (entity)
                    {
                        case Actor actor:
                            var selectedActor = selectedEntity as Actor;
                            selectedActor.OriginalRotation = actor.OriginalRotation;
                            selectedActor.Scale = actor.Scale;
                            break;
                        case Brush brush:
                            var selectedBrush = selectedEntity as Brush;
                            selectedBrush.OriginalRotation = brush.OriginalRotation;
                            selectedBrush.Scale = brush.Scale;
                            break;
                        case Light light:
                            var selectedLight = selectedEntity as Light;
                            selectedLight.Color = light.Color;
                            break;
                    }
                }
            }

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Enabled && _renderManager != null)
            {
                base.OnPaint(e);
                RenderFrame();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            if (Resolution != null)
            {
                Resolution.Width = Width;
                Resolution.Height = Height;

                if (_renderManager != null && _renderManager.IsLoaded)
                {
                    _renderManager.ResizeResolution();
                }
            }

            if (PanelSize != null)
            {
                PanelSize.Width = Width;
                PanelSize.Height = Height;

                if (_renderManager != null && _renderManager.IsLoaded)
                {
                    _renderManager.ResizeWindow();
                }
            }
        }

        private void RenderFrame()
        {
            MakeCurrent();

            switch (RenderMode)
            {
                case RenderModes.Wireframe:
                    _renderManager.RenderWireframe(_gameState.Camera, _gameState.EntityManager);
                    _renderManager.RenderEntityIDs(_gameState.Camera, _gameState.EntityManager);
                    break;
                case RenderModes.Diffuse:
                    _renderManager.RenderDiffuseFrame(_gameState.Camera, _gameState.EntityManager, _gameState.TextureManager);
                    _renderManager.RenderEntityIDs(_gameState.Camera, _gameState.EntityManager);
                    break;
                case RenderModes.Lit:
                    _renderManager.RenderLitFrame(_gameState.Camera, _gameState.EntityManager, _gameState.TextureManager);
                    _renderManager.RenderEntityIDs(_gameState.Camera, _gameState.EntityManager);
                    break;
                case RenderModes.Full:  
                    _renderManager.RenderFullFrame(_gameState.Camera, _gameState.EntityManager, _gameState.TextureManager);
                    break;
            }

            if (SelectedEntities.Count > 0)
            {
                _renderManager.RenderSelection(_gameState.Camera, SelectedEntities, TransformMode);
            }

            GL.UseProgram(0);
            SwapBuffers();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            IsMouseInPanel = true;
            base.OnMouseEnter(e);
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

        public void Zoom(int wheelDelta)
        {
            if (_gameState.Camera is OrthographicCamera camera)
            {
                var width = camera.Width - wheelDelta * 0.01f;

                if (width > 0.0f)
                {
                    camera.Width = width;
                    RenderFrame();
                }
            }
        }

        public void SelectEntity(Point coordinates, bool isMultiSelect)
        {
            var mouseCoordinates = new Vector2((float)coordinates.X - Location.X, Height - (float)coordinates.Y - Location.Y);

            RenderFrame();
            var id = _renderManager.GetEntityIDFromPoint(mouseCoordinates);

            if (id > 0)
            {
                var entity = _gameState.EntityManager.GetEntityByID(id);

                if (isMultiSelect && SelectedEntities.Select(e => e.ID).Contains(id))
                {
                    SelectedEntities.Remove(entity);
                    EntitySelectionChanged?.Invoke(this, new EntitiesEventArgs(SelectedEntities));
                    RenderFrame();
                }
                else if (!SelectionRenderer.IsReservedID(id) && !SelectedEntities.Select(e => e.ID).Contains(id))
                {
                    if (!isMultiSelect)
                    {
                        SelectedEntities.Clear();
                    }
                    
                    SelectedEntities.Add(entity);
                    EntitySelectionChanged?.Invoke(this, new EntitiesEventArgs(SelectedEntities));
                    RenderFrame();
                }
            }
            else if (!isMultiSelect)
            {
                SelectedEntities.Clear();

                EntitySelectionChanged?.Invoke(this, new EntitiesEventArgs(SelectedEntities));
                RenderFrame();
            }
        }

        public void ClearSelectedEntities()
        {
            SelectedEntities.Clear();
        }

        public void SetSelectionType()
        {
            Invoke(new Action(() =>
            {
                var id = _renderManager.GetEntityIDFromPoint(new Vector2(_currentMouseLocation.X, Height - _currentMouseLocation.Y));
                SelectionType = SelectionRenderer.GetSelectionTypeFromID(id);
            }));
        }

        public void StartDrag(Point location)
        {
            _startMouseLocation = location;
            IsDragging = true;
            _pollTimer.Start();
        }

        private void PollTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Handle user input, then poll their input for handling on the following frame
            HandleInput();
            PollForInput();
            
            if (_invalidated)
            {
                Invalidate();
                _invalidated = false;
            }
        }

        public void EndDrag()
        {
            IsDragging = false;
            _gameState._inputState.ClearState();
            SelectionType = SelectionTypes.None;
            _pollTimer.Stop();
        }

        private void HandleInput()
        {
            if (_gameState._inputState.IsReleased(new Input(MouseButton.Left)))
            {
                SelectionType = SelectionTypes.None;
            }

            if (_gameState._inputState.IsHeld(new Input(MouseButton.Left), new Input(MouseButton.Right)))
            {
                // Both mouse buttons allow "strafing"
                var right = Vector3.Cross(_gameState.Camera._viewMatrix.Up, _gameState.Camera._viewMatrix.LookAt - _gameState.Camera.Position).Normalized();

                var verticalTranslation = _gameState.Camera._viewMatrix.Up * _gameState._inputState.MouseDelta.Y * 0.02f;
                var horizontalTranslation = right * _gameState._inputState.MouseDelta.X * 0.02f;

                _gameState.Camera.Position -= verticalTranslation + horizontalTranslation;
                _gameState.Camera._viewMatrix.LookAt -= verticalTranslation + horizontalTranslation;

                //IsCursorVisible = false;
                SelectionType = SelectionTypes.None;
                _invalidated = true;
            }
            else if (SelectionType != SelectionTypes.None)
            {
                if (_gameState._inputState.IsHeld(new Input(MouseButton.Left)))
                {
                    // TODO - Can use entity's current rotation to determine position adjustment by that angle, rather than by MouseDelta.Y
                    foreach (var entity in SelectedEntities)
                    {
                        switch (TransformMode)
                        {
                            case TransformModes.Translate:
                                HandleEntityTranslation(entity);
                                break;
                            case TransformModes.Rotate:
                                HandleEntityRotation(entity);
                                break;
                            case TransformModes.Scale:
                                HandleEntityScale(entity);
                                break;
                        }
                    }

                    //IsCursorVisible = false;
                    Invoke(new Action(() => RenderFrame()));
                    Invoke(new Action(() => EntitySelectionChanged?.Invoke(this, new EntitiesEventArgs(SelectedEntities))));
                }
            }
            else if (ViewType == ViewTypes.Perspective)
            {
                if (_gameState._inputState.IsHeld(new Input(MouseButton.Left)))
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

                    //IsCursorVisible = false;
                    _invalidated = true;
                }
                else if (_gameState._inputState.IsHeld(new Input(MouseButton.Right)))
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
                    //IsCursorVisible = false;
                    _invalidated = true;
                }
            }
        }

        private void HandleEntityTranslation(IEntity entity)
        {
            var position = entity.Position;

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

            entity.Position = position;
        }

        private void HandleEntityRotation(IEntity entity)
        {
            if (entity is IRotate rotater)
            {
                var rotation = rotater.OriginalRotation;

                switch (SelectionType)
                {
                    case SelectionTypes.Red:
                        rotation.X -= _gameState._inputState.MouseDelta.Y * 0.002f;
                        break;
                    case SelectionTypes.Green:
                        rotation.Y -= _gameState._inputState.MouseDelta.Y * 0.002f;
                        break;
                    case SelectionTypes.Blue:
                        rotation.Z -= _gameState._inputState.MouseDelta.Y * 0.002f;
                        break;
                    case SelectionTypes.Cyan:
                        rotation.Y += _gameState._inputState.MouseDelta.X * 0.002f;
                        rotation.Z -= _gameState._inputState.MouseDelta.Y * 0.002f;
                        break;
                    case SelectionTypes.Magenta:
                        rotation.Z -= _gameState._inputState.MouseDelta.Y * 0.002f;
                        rotation.X += _gameState._inputState.MouseDelta.X * 0.002f;
                        break;
                    case SelectionTypes.Yellow:
                        rotation.X += _gameState._inputState.MouseDelta.X * 0.002f;
                        rotation.Y -= _gameState._inputState.MouseDelta.Y * 0.002f;
                        break;
                }

                rotater.OriginalRotation = rotation;
            }
        }

        private void HandleEntityScale(IEntity entity)
        {
            if (entity is IScale scaler)
            {
                var scale = scaler.Scale;

                switch (SelectionType)
                {
                    case SelectionTypes.Red:
                        scale.X -= _gameState._inputState.MouseDelta.Y * 0.002f;
                        break;
                    case SelectionTypes.Green:
                        scale.Y -= _gameState._inputState.MouseDelta.Y * 0.002f;
                        break;
                    case SelectionTypes.Blue:
                        scale.Z -= _gameState._inputState.MouseDelta.Y * 0.002f;
                        break;
                    case SelectionTypes.Cyan:
                        scale.Y += _gameState._inputState.MouseDelta.X * 0.002f;
                        scale.Z -= _gameState._inputState.MouseDelta.Y * 0.002f;
                        break;
                    case SelectionTypes.Magenta:
                        scale.Z -= _gameState._inputState.MouseDelta.Y * 0.002f;
                        scale.X += _gameState._inputState.MouseDelta.X * 0.002f;
                        break;
                    case SelectionTypes.Yellow:
                        scale.X += _gameState._inputState.MouseDelta.X * 0.002f;
                        scale.Y -= _gameState._inputState.MouseDelta.Y * 0.002f;
                        break;
                }

                scaler.Scale = scale;
            }
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            _currentMouseLocation = e.Location;

            if (IsDragging)
            {
                Mouse.SetPosition(_startMouseLocation.X, _startMouseLocation.Y);
            }
        }

        private void PollForInput() => _gameState._inputState.UpdateState(Keyboard.GetState(), Mouse.GetState());
    }
}
