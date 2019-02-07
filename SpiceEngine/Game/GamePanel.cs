using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SpiceEngine.Entities;
using SpiceEngine.Entities.Cameras;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Inputs;
using SpiceEngine.Maps;
using SpiceEngine.Outputs;
using SpiceEngine.Rendering.Processing;
using SpiceEngine.Utilities;
using Brush = SpiceEngine.Entities.Brushes.Brush;
using Timer = System.Timers.Timer;
using SpiceEngine.Rendering;
using SpiceEngine.Entities.Volumes;
using SpiceEngine.Entities.Actors;
using SpiceEngine.Physics;
using SpiceEngine.Scripting;
using SpiceEngine.Sounds;
using SpiceEngine.Rendering.Textures;
using System.IO;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Vertices;

namespace SpiceEngine.Game
{
    public enum Tools
    {
        Selector,
        Volume,
        Brush,
        Mesh,
        Texture
    }

    public class GamePanel : GLControl, IMouseDelta
    {
        public Resolution Resolution { get; private set; }
        public Resolution WindowSize { get; private set; }

        public double Frequency { get; private set; }
        public RenderModes RenderMode { get; set; }
        public TransformModes TransformMode { get; set; }
        public List<IEntity> SelectedEntities { get; private set; } = new List<IEntity>();
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
                        var mapVolume = MapVolume.RectangularPrism(Vector3.Zero, 10.0f, 10.0f, 10.0f);
                        //_toolVolume = Volume.RectangularPrism(Vector3.Zero, 10.0f, 10.0f, 10.0f, new Vector4(0.0f, 0.0f, 0.5f, 0.2f));
                        _toolVolume = mapVolume.ToEntity();
                        int entityID = _entityManager.AddEntity(_toolVolume);

                        lock (_loadLock)
                        {
                            if (_renderManager != null)
                            {
                                //var mesh = new Mesh3D<Simple3DVertex>(mapVolume.Vertices.Select(v => new Simple3DVertex(v)).ToList(), mapVolume.TriangleIndices);
                                var mesh = new Mesh3D<ColorVertex3D>(mapVolume.Vertices.Select(v => new ColorVertex3D(v, new Vector4(0.0f, 0.0f, 1.0f, 0.5f))).ToList(), mapVolume.TriangleIndices);
                                _renderManager.BatchManager.AddVolume(entityID, mesh);
                                _renderManager.BatchManager.Load(entityID);
                            }
                        }
                        break;
                    default:
                        if (_toolVolume != null)
                        {
                            _entityManager.RemoveEntityByID(_toolVolume.ID);

                            lock (_loadLock)
                            {
                                if (_renderManager != null)
                                {
                                    _renderManager.BatchManager.RemoveByEntityID(_toolVolume.ID);
                                    _toolVolume = null;
                                }
                            }
                        }
                        break;
                }

                Invalidate();
            }
        }

        public Vector2? MouseCoordinates { get; private set; }
        public bool IsMouseInWindow { get; private set; }

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
        public event EventHandler<DuplicatedEntityEventArgs> EntityDuplicated;

        private object _loadLock = new object();
        private bool _isLoaded = false;
        private Map _map;
        private EntityMapping _entityMapping;

        private PanelCamera _panelCamera;
        private EntityManager _entityManager;
        private TextureManager _textureManager;
        private InputManager _inputManager;
        //private SoundManager _soundManager;
        private RenderManager _renderManager;

        private bool _invalidated = false;
        private bool _isDuplicating = false;

        private Point _currentMouseLocation;
        private Point _startMouseLocation;
        private Timer _pollTimer = new Timer();

        private Tools _selectedTool = Tools.Brush;
        private Volume _toolVolume;

        private ViewTypes _viewType;

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
            WindowSize = new Resolution(Width, Height);

            //Console.WriteLine("GL Version: " + GL.GetString(StringName.Version));

            _pollTimer.Interval = 16.67;
            _pollTimer.Elapsed += PollTimer_Elapsed;
        }

        public void SetViewType(ViewTypes viewType) => _viewType = viewType;

        public void CenterView()
        {
            if (_entityManager != null && SelectedEntities.Count > 0)
            {
                _panelCamera.CenterView(SelectedEntities.Select(e => e.Position));
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

            _inputManager = new InputManager(this);
            _renderManager = new RenderManager(Resolution, WindowSize);

            lock (_loadLock)
            {
                _isLoaded = true;

                if (_map != null)
                {
                    //LoadBrushes(map.Brushes, entityMapping.BrushIDs);
                    //LoadActors(map.Actors, entityMapping.ActorIDs);
                    _panelCamera = new PanelCamera(Resolution, _renderManager)
                    {
                        ViewType = _viewType
                    };
                    _panelCamera.Load();

                    _renderManager = new RenderManager(Resolution, WindowSize);
                    _renderManager.LoadFromMap(_map, _entityManager, _entityMapping);

                    if (_selectedTool == Tools.Volume && _toolVolume != null)
                    {
                        // TODO - Correct this, shouldn't be creating another MapVolume here
                        var mapVolume = MapVolume.RectangularPrism(Vector3.Zero, 10.0f, 10.0f, 10.0f);
                        var mesh = new Mesh3D<ColorVertex3D>(mapVolume.Vertices.Select(v => new ColorVertex3D(v, new Vector4(0.0f, 0.0f, 1.0f, 0.5f))).ToList(), mapVolume.TriangleIndices);
                        _renderManager.BatchManager.AddVolume(_toolVolume.ID, mesh);
                        _renderManager.BatchManager.Load(_toolVolume.ID);
                    }

                    // Clear pointers
                    _map = null;
                    _entityMapping = null;

                    Invalidate();
                    IsLoaded = true;
                    PanelLoaded?.Invoke(this, new PanelLoadedEventArgs());
                }
            }

            base.OnLoad(e);
        }

        public void LoadFromMap(Map map, EntityManager entityManager, TextureManager textureManager, EntityMapping entityMapping)
        {
            lock (_loadLock)
            {
                _map = map;
                _entityManager = entityManager;
                _textureManager = textureManager;
                _entityMapping = entityMapping;

                if (_isLoaded)
                {
                    //LoadBrushes(map.Brushes, entityMapping.BrushIDs);
                    //LoadActors(map.Actors, entityMapping.ActorIDs);
                    _panelCamera = new PanelCamera(Resolution, _renderManager);
                    _panelCamera.ViewType = _viewType;
                    _panelCamera.Load();

                    _renderManager = new RenderManager(Resolution, WindowSize);
                    _renderManager.LoadFromMap(_map, _entityManager, _entityMapping);

                    // Clear pointers
                    _map = null;
                    _entityMapping = null;

                    Invalidate();
                    IsLoaded = true;
                    PanelLoaded?.Invoke(this, new PanelLoadedEventArgs());
                }
            }
        }

        /*public void LoadFromEntities(EntityManager entities, Map map)
        {
            entities.LoadEntities();
            _gameManager.LoadFromEntities(entities, map);
            LoadCamera();

            _renderManager.Load(_entityManager, map.SkyboxTextureFilePaths);
            Invalidate();

            IsLoaded = true;
            PanelLoaded?.Invoke(this, new PanelLoadedEventArgs());
        }*/

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
                var selectedEntity = _entityManager.GetEntity(entity.ID);

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
            if (Enabled && IsLoaded && _renderManager != null && _renderManager.IsLoaded && _entityManager != null)
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

            if (WindowSize != null)
            {
                WindowSize.Width = Width;
                WindowSize.Height = Height;

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
                    _renderManager.RenderWireframe(_entityManager, _panelCamera.Camera);
                    _renderManager.RenderEntityIDs(_entityManager, _panelCamera.Camera);
                    break;
                case RenderModes.Diffuse:
                    _renderManager.RenderDiffuseFrame(_entityManager, _panelCamera.Camera, _textureManager);
                    _renderManager.RenderEntityIDs(_entityManager, _panelCamera.Camera);
                    break;
                case RenderModes.Lit:
                    _renderManager.RenderLitFrame(_entityManager, _panelCamera.Camera, _textureManager);
                    _renderManager.RenderEntityIDs(_entityManager, _panelCamera.Camera);
                    break;
                case RenderModes.Full:  
                    _renderManager.RenderFullFrame(_entityManager, _panelCamera.Camera, _textureManager);
                    break;
            }

            if (SelectedEntities.Count > 0)
            {
                _renderManager.RenderSelection(_entityManager, _panelCamera.Camera, SelectedEntities, TransformMode);
            }

            GL.UseProgram(0);
            SwapBuffers();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            IsMouseInWindow = true;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            IsMouseInWindow = false;
            base.OnMouseLeave(e);
        }

        public void Zoom(int wheelDelta)
        {
            if (_panelCamera.Zoom(wheelDelta))
            {
                RenderFrame();
            }
        }

        public void SelectEntity(Point coordinates, bool isMultiSelect)
        {
            var mouseCoordinates = new Vector2((float)coordinates.X - Location.X, Height - (float)coordinates.Y - Location.Y);

            RenderFrame();
            var id = _renderManager.GetEntityIDFromPoint(mouseCoordinates);

            if (id > 0)
            {
                var entity = _entityManager.GetEntity(id);

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
            _pollTimer.Stop();

            // Handle user input, then poll their input for handling on the following frame
            HandleInput();
            _inputManager.Update();
            
            if (_invalidated)
            {
                Invalidate();
                _invalidated = false;
            }

            if (IsDragging)
            {
                _pollTimer.Start();
            }
        }

        public bool IsHeld()
        {
            _inputManager.Update();
            return _inputManager.IsDown(new Input(MouseButton.Left)) || _inputManager.IsDown(new Input(MouseButton.Right));
        }

        public void EndDrag()
        {
            IsDragging = false;
            _inputManager.Clear();
            SelectionType = SelectionTypes.None;
            _pollTimer.Stop();
        }

        private void DuplicateSelectedItems()
        {
            // Create duplicates and overwrite SelectedEntities with them
            var duplicateEntities = new List<IEntity>();

            foreach (var entity in SelectedEntities)
            {
                var duplicateEntity = _entityManager.DuplicateEntity(entity);
                // Need to duplicate colliders
                // Need to duplicate scripts

                EntityDuplicated?.Invoke(this, new DuplicatedEntityEventArgs(entity.ID, duplicateEntity.ID));
                duplicateEntities.Add(duplicateEntity);
            }

            SelectedEntities = duplicateEntities;
        }

        public void Duplicate(int entityID, int duplicateEntityID)
        {
            Invoke(new Action(() => _renderManager.BatchManager.DuplicateBatch(entityID, duplicateEntityID)));
        }

        private void HandleInput()
        {
            if (_isDuplicating && !_inputManager.IsDown(new Input(Key.ShiftLeft)))
            {
                _isDuplicating = false;
            }

            if (_inputManager.IsReleased(new Input(MouseButton.Left)))
            {
                SelectionType = SelectionTypes.None;
            }

            if (_inputManager.IsDown(new Input(MouseButton.Left)) && _inputManager.IsDown(new Input(MouseButton.Right)))
            {
                _panelCamera.Strafe(_inputManager.MouseDelta);

                //IsCursorVisible = false;
                SelectionType = SelectionTypes.None;
                _invalidated = true;
            }
            else if (SelectionType != SelectionTypes.None)
            {
                if (_inputManager.IsDown(new Input(MouseButton.Left)))
                {
                    if (!_isDuplicating && _inputManager.IsDown(new Input(Key.ShiftLeft)))
                    {
                        _isDuplicating = true;
                        DuplicateSelectedItems();
                    }

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
            else if (_panelCamera.ViewType == ViewTypes.Perspective)
            {
                if (_inputManager.IsDown(new Input(MouseButton.Left)))
                {
                    _panelCamera.Travel(_inputManager.MouseDelta);

                    //IsCursorVisible = false;
                    _invalidated = true;
                }
                else if (_inputManager.IsDown(new Input(MouseButton.Right)))
                {
                    _panelCamera.Turn(_inputManager.MouseDelta);

                    //IsCursorVisible = false;
                    _invalidated = true;
                }
                else if (_inputManager.IsDown(new Input(MouseButton.Button1)))
                {
                    if (SelectedEntities.Count > 0)
                    {
                        var pivotPosition = new Vector3()
                        {
                            X = SelectedEntities.Average(e => e.Position.X),
                            Y = SelectedEntities.Average(e => e.Position.Y),
                            Z = SelectedEntities.Average(e => e.Position.Z)
                        };

                        _panelCamera.Pivot(_inputManager.MouseDelta, _inputManager.MouseWheelDelta, pivotPosition);

                        //IsCursorVisible = false;
                        _invalidated = true;
                    }
                }
            }
        }

        private void HandleEntityTranslation(IEntity entity)
        {
            var position = entity.Position;

            switch (SelectionType)
            {
                case SelectionTypes.Red:
                    position.X -= _inputManager.MouseDelta.Y * 0.002f;
                    break;
                case SelectionTypes.Green:
                    position.Y -= _inputManager.MouseDelta.Y * 0.002f;
                    break;
                case SelectionTypes.Blue:
                    position.Z -= _inputManager.MouseDelta.Y * 0.002f;
                    break;
                case SelectionTypes.Cyan:
                    position.Y += _inputManager.MouseDelta.X * 0.002f;
                    position.Z -= _inputManager.MouseDelta.Y * 0.002f;
                    break;
                case SelectionTypes.Magenta:
                    position.Z -= _inputManager.MouseDelta.Y * 0.002f;
                    position.X += _inputManager.MouseDelta.X * 0.002f;
                    break;
                case SelectionTypes.Yellow:
                    position.X += _inputManager.MouseDelta.X * 0.002f;
                    position.Y -= _inputManager.MouseDelta.Y * 0.002f;
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
                        rotation.X -= _inputManager.MouseDelta.Y * 0.002f;
                        break;
                    case SelectionTypes.Green:
                        rotation.Y -= _inputManager.MouseDelta.Y * 0.002f;
                        break;
                    case SelectionTypes.Blue:
                        rotation.Z -= _inputManager.MouseDelta.Y * 0.002f;
                        break;
                    case SelectionTypes.Cyan:
                        rotation.Y += _inputManager.MouseDelta.X * 0.002f;
                        rotation.Z -= _inputManager.MouseDelta.Y * 0.002f;
                        break;
                    case SelectionTypes.Magenta:
                        rotation.Z -= _inputManager.MouseDelta.Y * 0.002f;
                        rotation.X += _inputManager.MouseDelta.X * 0.002f;
                        break;
                    case SelectionTypes.Yellow:
                        rotation.X += _inputManager.MouseDelta.X * 0.002f;
                        rotation.Y -= _inputManager.MouseDelta.Y * 0.002f;
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
                        scale.X -= _inputManager.MouseDelta.Y * 0.002f;
                        break;
                    case SelectionTypes.Green:
                        scale.Y -= _inputManager.MouseDelta.Y * 0.002f;
                        break;
                    case SelectionTypes.Blue:
                        scale.Z -= _inputManager.MouseDelta.Y * 0.002f;
                        break;
                    case SelectionTypes.Cyan:
                        scale.Y += _inputManager.MouseDelta.X * 0.002f;
                        scale.Z -= _inputManager.MouseDelta.Y * 0.002f;
                        break;
                    case SelectionTypes.Magenta:
                        scale.Z -= _inputManager.MouseDelta.Y * 0.002f;
                        scale.X += _inputManager.MouseDelta.X * 0.002f;
                        break;
                    case SelectionTypes.Yellow:
                        scale.X += _inputManager.MouseDelta.X * 0.002f;
                        scale.Y -= _inputManager.MouseDelta.Y * 0.002f;
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
    }
}
