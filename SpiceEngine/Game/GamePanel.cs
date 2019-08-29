using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using SpiceEngine.Entities;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Entities.Selection;
using SpiceEngine.Entities.Volumes;
using SpiceEngine.Inputs;
using SpiceEngine.Maps;
using SpiceEngine.Outputs;
using SpiceEngine.Rendering;
using SpiceEngine.Rendering.Processing;
using SpiceEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

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

    public class GamePanel : GLControl, IMouseTracker
    {
        private PanelCamera _panelCamera;

        private IEntityProvider _entityProvider;
        private IInputProvider _inputProvider;

        private Map _map;
        private EntityMapping _entityMapping;
        private RenderManager _renderManager;

        private bool _invalidated = false;
        private bool _isDuplicating = false;

        private Point _currentMouseLocation;
        private Point _startMouseLocation;
        private Timer _pollTimer = new Timer(); 

        private Tools _selectedTool = Tools.Brush;
        private Volume _toolVolume;

        private bool _isLoaded = false;
        private object _loadLock = new object();

        private TransformModes _transformMode;
        private RenderModes _renderMode;
        private ViewTypes _viewType;

        public TransformModes TransformMode
        {
            get => _transformMode;
            set
            {
                _transformMode = value;
                Invalidate();
            }
        }

        public RenderModes RenderMode
        {
            get => _renderMode;
            set
            {
                _renderMode = value;

                lock (_loadLock)
                {
                    if (_renderManager != null)
                    {
                        _renderManager.RenderMode = value;
                    }
                }

                Invalidate();
            }
        }

        public ViewTypes ViewType
        {
            get => _viewType;
            set
            {
                lock (_loadLock)
                {
                    _viewType = value;

                    if (_isLoaded)
                    {
                        _panelCamera.ViewType = value;
                    }
                }
            }
        }

        public Resolution Resolution { get; private set; }
        public Resolution WindowSize { get; private set; }

        public double Frequency { get; private set; }
        public SelectionManager SelectionManager { get; private set; }
        public Tools SelectedTool
        {
            get => _selectedTool;
            set
            {
                _selectedTool = value;

                switch (_selectedTool)
                {
                    case Tools.Volume:
                        //var mapVolume = MapVolume.Box(Vector3.Zero, 10.0f, 10.0f, 10.0f);
                        //_toolVolume = Volume.Box(Vector3.Zero, 10.0f, 10.0f, 10.0f, new Vector4(0.0f, 0.0f, 0.5f, 0.2f));
                        //_toolVolume = mapVolume.ToEntity();
                        /*int entityID = _entityProvider.AddEntity(_toolVolume);

                        lock (_loadLock)
                        {
                            if (_renderManager != null)
                            {
                                //var mesh = new Mesh3D<Simple3DVertex>(mapVolume.Vertices.Select(v => new Simple3DVertex(v)).ToList(), mapVolume.TriangleIndices);
                                //var mesh = new Mesh3D<ColorVertex3D>(mapVolume.Vertices.Select(v => new ColorVertex3D(v, new Color4(0.0f, 0.0f, 1.0f, 0.5f))).ToList(), mapVolume.TriangleIndices);
                                //_renderManager.BatchManager.AddVolume(entityID, mesh);
                                _renderManager.BatchManager.Load(entityID);
                            }
                        }*/
                        break;
                    default:
                        if (_toolVolume != null)
                        {
                            _entityProvider.RemoveEntityByID(_toolVolume.ID);

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
        public event EventHandler<EntitiesEventArgs> EntitySelectionChanged;
        public event EventHandler<DuplicationEventArgs> EntityDuplicated;

        public GamePanel() : base(GraphicsMode.Default, 3, 0, GraphicsContextFlags.ForwardCompatible)
        {
            Resolution = new Resolution(Width, Height);
            WindowSize = new Resolution(Width, Height);

            //Console.WriteLine("GL Version: " + GL.GetString(StringName.Version));

            _pollTimer.Interval = 16.67;
            _pollTimer.Elapsed += PollTimer_Elapsed;
        }

        public void CenterView()
        {
            if (_entityProvider != null && SelectionManager.SelectionCount > 0)
            {
                _panelCamera.CenterView(SelectionManager.Position);
                Invalidate();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            Location = new Point(0, 0);

            lock (_loadLock)
            {
                _isLoaded = true;

                if (_entityProvider != null && _inputProvider != null && _entityMapping != null)
                {
                    LoadFromGameManager();
                }
            }

            base.OnLoad(e);
        }

        public void LoadGameManager(GameManager gameManager, Map map, EntityMapping entityMapping)
        {
            _entityProvider = gameManager.EntityManager;
            _inputProvider = gameManager.InputManager;
            SelectionManager = new SelectionManager(_entityProvider);

            _map = map;
            _entityMapping = entityMapping;

            lock (_loadLock)
            {
                if (_isLoaded)
                {
                    LoadFromGameManager();
                }
            }
        }

        private void LoadFromGameManager()
        {
            _renderManager = new RenderManager(Resolution, WindowSize)
            {
                IsInEditorMode = true,
                RenderMode = _renderMode
            };
            _renderManager.SetEntityProvider(_entityProvider);
            _renderManager.LoadFromMap(_map/*, _entityMapping*/);

            _panelCamera = new PanelCamera(Resolution, _renderManager)
            {
                ViewType = ViewType
            };
            _panelCamera.Load();
            _renderManager.SetSelectionProvider(SelectionManager);
            _renderManager.SetCamera(_panelCamera.Camera);

            // Clear pointers
            //_map = null;
            //_entityMapping = null;

            /*if (_selectedTool == Tools.Volume && _toolVolume != null)
            {
                // TODO - Correct this, shouldn't be creating another MapVolume here
                var mapVolume = MapVolume.Box(Vector3.Zero, 10.0f, 10.0f, 10.0f);
                var mesh = new Mesh3D<ColorVertex3D>(mapVolume.Vertices.Select(v => new ColorVertex3D(v, new Color4(0.0f, 0.0f, 1.0f, 0.5f))).ToList(), mapVolume.TriangleIndices);
                _renderManager.BatchManager.AddVolume(_toolVolume.ID, mesh);
                _renderManager.BatchManager.Load(_toolVolume.ID);
            }*/

            // Default to allowing all rendered entities to be selectable
            //SelectionManager.SetSelectable(_entityProvider.EntityRenderIDs);

            Invalidate();
            IsLoaded = true;
            PanelLoaded?.Invoke(this, new PanelLoadedEventArgs());
        }

        public void AddEntity(int entityID, IRenderable renderable) => _renderManager?.AddEntity(entityID, renderable);
        public void RemoveEntity(int entityID) => _renderManager?.RemoveEntity(entityID);

        // TODO - Make this method less janky and terrible
        public void DoLoad() => _renderManager?.BatchManager.Load();

        public void SelectEntity(IEntity entity)
        {
            _renderManager?.SetSelected(entity.ID.Yield());

            SelectionManager.Select(entity.ID);
            Invalidate();
        }

        public void SelectEntities(IEnumerable<IEntity> entities)
        {
            if (entities.Any())
            {
                _renderManager?.SetSelected(entities.Select(e => e.ID));
                SelectionManager.Select(entities.Select(e => e.ID));
            }
            else
            {
                _renderManager?.SetDeselected(SelectionManager.SelectedIDs);
                SelectionManager.ClearSelection();
            }
            
            Invalidate();
        }

        public void DelayAction(int nTicks, Action action)
        {
            // TODO - Handle this better -> We aren't even keeping a reference to this...
            var delayedAction = new DelayedUpdate((IUpdate)_renderManager, nTicks, action);
        }

        public void UpdateEntities(IEnumerable<IEntity> entities)
        {
            foreach (var entity in entities)
            {
                var selectedEntity = _entityProvider.GetEntity(entity.ID);

                if (selectedEntity != null)
                {
                    selectedEntity.Position = entity.Position;

                    if (entity is IRotate rotator && selectedEntity is IRotate selectedRotator)
                    {
                        selectedRotator.Rotation = rotator.Rotation;
                    }

                    if (entity is IScale scaler && selectedEntity is IScale selectedScaler)
                    {
                        selectedScaler.Scale = scaler.Scale;
                    }

                    if (entity is ILight light && selectedEntity is ILight selectedLight)
                    {
                        selectedLight.Color = light.Color;
                    }
                }
            }

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Enabled && IsLoaded && _renderManager != null && _renderManager.IsLoaded && _entityProvider != null)
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

            _renderManager.Tick();

            // TODO - Determine how to handle this
            if (SelectionManager.SelectionCount > 0)
            {
                // This is still necessary right now for rendering lights and transform arrows...
                _renderManager.RenderSelection(SelectionManager.SelectedEntities, TransformMode);
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
                Invalidate();
            }
        }

        public void SetWireframeThickness(float thickness)
        {
            _renderManager.SetWireframeThickness(thickness);
            Invalidate();
        }

        public void SetWireframeColor(Color4 color)
        {
            _renderManager.SetWireframeColor(color);
            Invalidate();
        }

        public void SetSelectedWireframeThickness(float thickness)
        {
            _renderManager.SetSelectedWireframeThickness(thickness);
            Invalidate();
        }

        public void SetSelectedWireframeColor(Color4 color)
        {
            _renderManager.SetSelectedWireframeColor(color);
            Invalidate();
        }

        public void SetSelectedLightWireframeThickness(float thickness)
        {
            _renderManager.SetSelectedLightWireframeThickness(thickness);
            Invalidate();
        }

        public void SetSelectedLightWireframeColor(Color4 color)
        {
            _renderManager.SetSelectedLightWireframeColor(color);
            Invalidate();
        }

        public void SetGridLineThickness(float thickness)
        {
            _renderManager.SetGridLineThickness(thickness);
            Invalidate();
        }

        public void SetGridUnitColor(Color4 color)
        {
            _renderManager.SetGridUnitColor(color);
            Invalidate();
        }

        public void SetGridAxisColor(Color4 color)
        {
            _renderManager.SetGridAxisColor(color);
            Invalidate();
        }

        public void SetGrid5Color(Color4 color)
        {
            _renderManager.SetGrid5Color(color);
            Invalidate();
        }

        public void SetGrid10Color(Color4 color)
        {
            _renderManager.SetGrid10Color(color);
            Invalidate();
        }

        public void SetGridUnit(float unit)
        {
            _renderManager.SetGridUnit(unit);
            Invalidate();
        }

        public void SelectEntity(Point coordinates, bool isMultiSelect)
        {
            var mouseCoordinates = new Vector2((float)coordinates.X - Location.X, Height - (float)coordinates.Y - Location.Y);

            RenderFrame();
            var id = _renderManager.GetEntityIDFromPoint(mouseCoordinates);

            if (!SelectionRenderer.IsReservedID(id))
            {
                if (id > 0)
                {
                    if (isMultiSelect && SelectionManager.IsSelected(id))
                    {
                        _renderManager.SetDeselected(id.Yield());
                        SelectionManager.Remove(id);//.Deselect(id);
                        EntitySelectionChanged?.Invoke(this, new EntitiesEventArgs(SelectionManager.SelectedEntities));
                        Invalidate();
                    }
                    else if (!SelectionManager.IsSelected(id))
                    {
                        if (!isMultiSelect)
                        {
                            _renderManager.SetDeselected(SelectionManager.SelectedIDs);
                            SelectionManager.ClearSelection();
                        }

                        var entity = _entityProvider.GetEntityOrDefault(id);
                        if (entity != null)
                        {
                            _renderManager.SetSelected(id.Yield());
                            SelectionManager.Select(id);
                        }
                        else
                        {
                            _renderManager.SetDeselected(SelectionManager.SelectedIDs);
                            SelectionManager.ClearSelection();
                        }
                        
                        EntitySelectionChanged?.Invoke(this, new EntitiesEventArgs(SelectionManager.SelectedEntities));
                        Invalidate();
                    }
                }
                else if (!isMultiSelect)
                {
                    _renderManager.SetDeselected(SelectionManager.SelectedIDs);
                    SelectionManager.ClearSelection();

                    EntitySelectionChanged?.Invoke(this, new EntitiesEventArgs(SelectionManager.SelectedEntities));
                    Invalidate();
                }
            }
        }

        public void ClearSelectedEntities() => SelectionManager.Clear();

        public void SetSelectionType()
        {
            Invoke(new Action(() =>
            {
                var id = _renderManager.GetEntityIDFromPoint(new Vector2(_currentMouseLocation.X, Height - _currentMouseLocation.Y));
                SelectionManager.SelectionType = SelectionRenderer.GetSelectionTypeFromID(id);
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
            _inputProvider.Tick();

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
            _inputProvider.Tick();
            return _inputProvider.IsDown(new Input(MouseButton.Left)) || _inputProvider.IsDown(new Input(MouseButton.Right));
        }

        public void EndDrag()
        {
            IsDragging = false;
            _inputProvider.Clear();
            SelectionManager.SelectionType = SelectionTypes.None;
            _pollTimer.Stop();
        }

        public void Duplicate(int entityID, int duplicateEntityID) =>
            Invoke(new Action(() => _renderManager.BatchManager.DuplicateBatch(entityID, duplicateEntityID)));

        public void SetLayerOrSomeShit(string layerName)
        {
            //_entityProvider.EntityRenderIDs;

            // We have four new entity types to deal with for model rendering...
            // ShapeEntity -> Maps to a single mesh
            // FaceEntity -> Maps to a face in MeshBuild
            // TriangleEntity -> Maps to a triangle in MeshBuild
            // VertexEntity -> Maps to a vertex billboard texture

            // We have a few options...
            // When we first open a ModelComponent, we should create ALL necessary entities and split them into the four layers
            // We then need to give access to 
        }

        private void HandleInput()
        {
            if (_isDuplicating && !_inputProvider.IsDown(new Input(Key.ShiftLeft)))
            {
                _isDuplicating = false;
            }

            if (_inputProvider.IsReleased(new Input(MouseButton.Left)))
            {
                SelectionManager.SelectionType = SelectionTypes.None;
            }

            if (_inputProvider.IsDown(new Input(MouseButton.Left)) && _inputProvider.IsDown(new Input(MouseButton.Right)))
            {
                _panelCamera.Strafe(_inputProvider.MouseDelta);

                //IsCursorVisible = false;
                SelectionManager.SelectionType = SelectionTypes.None;
                _invalidated = true;
            }
            else if (SelectionManager.SelectionType != SelectionTypes.None)
            {
                if (_inputProvider.IsDown(new Input(MouseButton.Left)))
                {
                    if (!_isDuplicating && _inputProvider.IsDown(new Input(Key.ShiftLeft)))
                    {
                        _isDuplicating = true;

                        /*foreach (var duplication in SelectionManager.DuplicateSelection())
                        {
                            EntityDuplicated?.Invoke(this, new DuplicationEventArgs(duplication));
                        }*/
                    }

                    SelectionManager.HandleEntityTransforms(TransformMode, _inputProvider.MouseDelta);

                    Invalidate();
                    Invoke(new Action(() => EntitySelectionChanged?.Invoke(this, new EntitiesEventArgs(SelectionManager.SelectedEntities))));
                }
            }
            else if (_panelCamera.ViewType == ViewTypes.Perspective)
            {
                if (_inputProvider.IsDown(new Input(MouseButton.Left)))
                {
                    _panelCamera.Travel(_inputProvider.MouseDelta);
                    _invalidated = true;
                }
                else if (_inputProvider.IsDown(new Input(MouseButton.Right)))
                {
                    _panelCamera.Turn(_inputProvider.MouseDelta);
                    _invalidated = true;
                }
                else if (_inputProvider.IsDown(new Input(MouseButton.Button1)))
                {
                    if (SelectionManager.SelectionCount > 0)
                    {
                        _panelCamera.Pivot(_inputProvider.MouseDelta, _inputProvider.MouseWheelDelta, SelectionManager.Position);
                        _invalidated = true;
                    }
                }
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
