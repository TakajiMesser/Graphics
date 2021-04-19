using CitrusAnimationCore.Animations;
//using OpenTK.Graphics;
//using OpenTK.Graphics.OpenGL;
//using OpenTK.Input;
using SpiceEngine.Entities.Selection;
using SpiceEngine.Maps;
using SpiceEngine.Rendering;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Volumes;
using SpiceEngineCore.Game;
using SpiceEngineCore.Maps;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Utilities;
using StarchUICore;
using SweetGraphicsCore.Renderers.Processing;
using SweetGraphicsCore.Selection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TangyHIDCore;
using TangyHIDCore.Outputs;
using Color4 = SpiceEngineCore.Geometry.Color4;
//using GLControl = OpenTK.GLControl;
using Timer = System.Timers.Timer;
using Vector2 = SpiceEngineCore.Geometry.Vector2;

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

    /*public class GameControl : GLControl, IMouseTracker, IInvoker
    {
        private PanelCamera _panelCamera;

        private IEntityProvider _entityProvider;
        private IInputProvider _inputProvider;
        private IAnimationProvider _animationProvider;
        private IUIProvider _uiProvider;

        private Map _map;

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

        public EditorRenderManager RenderManager { get; private set; }

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
                    if (RenderManager != null)
                    {
                        RenderManager.RenderMode = value;
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

        public Display Display { get; private set; }

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
                            if (RenderManager != null)
                            {
                                //var mesh = new Mesh3D<Simple3DVertex>(mapVolume.Vertices.Select(v => new Simple3DVertex(v)).ToList(), mapVolume.TriangleIndices);
                                //var mesh = new Mesh3D<ColorVertex3D>(mapVolume.Vertices.Select(v => new ColorVertex3D(v, new Color4(0.0f, 0.0f, 1.0f, 0.5f))).ToList(), mapVolume.TriangleIndices);
                                //RenderManager.BatchManager.AddVolume(entityID, mesh);
                                RenderManager.BatchManager.Load(entityID);
                            }
                        }*
                        break;
                    default:
                        if (_toolVolume != null)
                        {
                            _entityProvider.RemoveEntityByID(_toolVolume.ID);

                            lock (_loadLock)
                            {
                                if (RenderManager != null)
                                {
                                    RenderManager.RemoveEntity(_toolVolume.ID);
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
        public Vector2? RelativeCoordinates { get; private set; }
        public bool IsMouseInWindow { get; private set; }
        public Resolution WindowSize => Display.Window;

        public bool IsLoaded { get; private set; }
        public bool IsDragging { get; private set; }
        public bool RenderGrid
        {
            get => RenderManager.RenderGrid;
            set
            {
                RenderManager.RenderGrid = value;
                Invalidate();
            }
        }

        public event EventHandler<PanelLoadedEventArgs> PanelLoaded;
        public event EventHandler<EntitiesEventArgs> EntitySelectionChanged;
        public event EventHandler<DuplicationEventArgs> EntityDuplicated;

        public GameControl() : base(GraphicsMode.Default, 3, 0, GraphicsContextFlags.ForwardCompatible)
        {
            Display = new Display(Width, Height);

            //Console.WriteLine("GL Version: " + GL.GetString(StringName.Version));

            _pollTimer.Interval = 16.67;
            _pollTimer.Elapsed += PollTimer_Elapsed;
        }

        public void CenterView()
        {
            if (_entityProvider != null && _panelCamera != null && SelectionManager.SelectionCount > 0)
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

                if (_entityProvider != null && _inputProvider != null)
                {
                    LoadFromSimulation();
                }
            }

            base.OnLoad(e);
        }

        public void LoadSimulation(SimulationManager simulationManager, IMap map)
        {
            _entityProvider = simulationManager.EntityProvider;
            _inputProvider = simulationManager.InputManager;
            _animationProvider = simulationManager.AnimationSystem;
            _uiProvider = simulationManager.UISystem;

            SelectionManager = new SelectionManager(_entityProvider);

            // TODO - Abstract out skybox texture provider to avoid needing map here
            _map = map as Map;

            lock (_loadLock)
            {
                if (_isLoaded)
                {
                    LoadFromSimulation();
                }
            }
        }

        public Task InvokeAsync(Action action) => Task.Run(() =>
        {
            Invoke((Action)(() =>
            {
                MakeCurrent();
                action();
            }));
        });

        public void InvokeSync(Action action)
        {
            Invoke((Action)(() =>
            {
                MakeCurrent();
                action();
            }));
        }

        public void ForceUpdate() => Invalidate();

        public Task<object> InvokeAsync(Func<object> func) => Task.Run(() =>
        {
            return Invoke((Func<object>)(() =>
            {
                MakeCurrent();
                return func();
            }));
        });

        public T InvokeSync<T>(Func<T> func) => (T)Invoke((Func<object>)(() =>
        {
            MakeCurrent();
            return func();
        }));

        private void LoadFromSimulation()
        {
            // TODO - Should we run this all on the UI thread?
            InvokeSync(() =>
            {
                //MakeCurrent();
                _panelCamera = new PanelCamera(Display.Resolution, _entityProvider)
                {
                    ViewType = ViewType
                };

                RenderManager = new EditorRenderManager(null, Display, _panelCamera)
                {
                    RenderMode = _renderMode,
                    Invoker = this
                };

                _panelCamera.GridRenderer = RenderManager;
                _panelCamera.Load();
                
                //RenderManager.LoadFromMap(_map/*, _entityMapping*);
                RenderManager.SetEntityProvider(_entityProvider);
                RenderManager.SetAnimationProvider(_animationProvider);
                RenderManager.SetUIProvider(_uiProvider);
                RenderManager.SetSelectionProvider(SelectionManager);

                // TODO - Can we load the map here?
                RenderManager.LoadFromMap(_map);

                // Clear pointers
                //_map = null;
                //_entityMapping = null;

                /*if (_selectedTool == Tools.Volume && _toolVolume != null)
                {
                    // TODO - Correct this, shouldn't be creating another MapVolume here
                    var mapVolume = MapVolume.Box(Vector3.Zero, 10.0f, 10.0f, 10.0f);
                    var mesh = new Mesh3D<ColorVertex3D>(mapVolume.Vertices.Select(v => new ColorVertex3D(v, new Color4(0.0f, 0.0f, 1.0f, 0.5f))).ToList(), mapVolume.TriangleIndices);
                    RenderManager.BatchManager.AddVolume(_toolVolume.ID, mesh);
                    RenderManager.BatchManager.Load(_toolVolume.ID);
                }*

                // Default to allowing all rendered entities to be selectable
                //SelectionManager.SetSelectable(_entityProvider.EntityRenderIDs);

                Invalidate();
                IsLoaded = true;
                PanelLoaded?.Invoke(this, new PanelLoadedEventArgs());
            });
        }

        // TODO - Fix this call...
        public void AddEntity(int entityID, IRenderable renderable) { }// => RenderManager?.AddEntity(entityID, renderable);
        public void RemoveEntity(int entityID) => RenderManager?.RemoveEntity(entityID);

        // TODO - Make this method less janky and terrible
        public void DoLoad() => RenderManager?.LoadBatcher();

        public void SelectEntity(int id)
        {
            RenderManager?.SetSelected(id.Yield());

            SelectionManager.Select(id);
            Invalidate();
        }

        public void SelectEntities(IEnumerable<int> ids)
        {
            // TODO - This should get queued even if the control isn't loaded yet
            if (SelectionManager == null) return;

            if (ids.Any())
            {
                RenderManager?.SetDeselected(SelectionManager.SelectedIDs);
                SelectionManager.ClearSelection();

                RenderManager?.SetSelected(ids);
                SelectionManager.Select(ids);
            }
            else
            {
                RenderManager?.SetDeselected(SelectionManager.SelectedIDs);
                SelectionManager.ClearSelection();
            }
            
            Invalidate();
        }

        public void DelayAction(int nTicks, Action action)
        {
            // TODO - Handle this better -> We aren't even keeping a reference to this...
            var delayedAction = new DelayedUpdate((IUpdate)RenderManager, nTicks, action);
        }

        public void UpdateEntities(IEnumerable<IEntity> entities)
        {
            // TODO - This should get queued even if the control isn't loaded yet
            if (SelectionManager == null) return;

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
            if (Enabled && IsLoaded && RenderManager != null && RenderManager.IsLoaded && _entityProvider != null)
            {
                base.OnPaint(e);
                RenderFrame();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            Display?.Resolution.Update(Width, Height);
            Display?.Window.Update(Width, Height);
        }

        //private static object _glContextLock = new object();

        private void RenderFrame()
        {
            //lock (_glContextLock)
            //{
                MakeCurrent();

                RenderManager.Tick();

                // TODO - Determine how to handle this
                if (SelectionManager.SelectionCount > 0)
                {
                    // This is still necessary right now for rendering lights and transform arrows...
                    RenderManager.RenderSelection(SelectionManager.SelectedEntities, TransformMode);
                }

                GL.UseProgram(0);
                SwapBuffers();
            //}
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
            RenderManager.SetWireframeThickness(thickness);
            Invalidate();
        }

        public void SetWireframeColor(Color4 color)
        {
            RenderManager.SetWireframeColor(color);
            Invalidate();
        }

        public void SetSelectedWireframeThickness(float thickness)
        {
            RenderManager.SetSelectedWireframeThickness(thickness);
            Invalidate();
        }

        public void SetSelectedWireframeColor(Color4 color)
        {
            RenderManager.SetSelectedWireframeColor(color);
            Invalidate();
        }

        public void SetSelectedLightWireframeThickness(float thickness)
        {
            RenderManager.SetSelectedLightWireframeThickness(thickness);
            Invalidate();
        }

        public void SetSelectedLightWireframeColor(Color4 color)
        {
            RenderManager.SetSelectedLightWireframeColor(color);
            Invalidate();
        }

        public void SetGridLineThickness(float thickness)
        {
            RenderManager.SetGridLineThickness(thickness);
            Invalidate();
        }

        public void SetGridUnitColor(Color4 color)
        {
            RenderManager.SetGridUnitColor(color);
            Invalidate();
        }

        public void SetGridAxisColor(Color4 color)
        {
            RenderManager.SetGridAxisColor(color);
            Invalidate();
        }

        public void SetGrid5Color(Color4 color)
        {
            RenderManager.SetGrid5Color(color);
            Invalidate();
        }

        public void SetGrid10Color(Color4 color)
        {
            RenderManager.SetGrid10Color(color);
            Invalidate();
        }

        public void SetGridUnit(float unit)
        {
            RenderManager.SetGridUnit(unit);
            Invalidate();
        }

        public int GetEntityIDFromPoint(Point coordinates)
        {
            RenderFrame();
            return RenderManager.GetEntityIDFromSelection(new Vector2(coordinates.X - Location.X, coordinates.Y - Location.Y));
        }

        public void SelectEntity(Point coordinates, bool isMultiSelect)
        {
            var id = GetEntityIDFromPoint(coordinates);

            if (!SelectionRenderer.IsReservedID(id))
            {
                if (id > 0)
                {
                    if (isMultiSelect && SelectionManager.IsSelected(id))
                    {
                        RenderManager.SetDeselected(id.Yield());
                        SelectionManager.Remove(id);//.Deselect(id);
                        EntitySelectionChanged?.Invoke(this, new EntitiesEventArgs(SelectionManager.SelectedEntities));
                        Invalidate();
                    }
                    else if (!SelectionManager.IsSelected(id))
                    {
                        if (!isMultiSelect)
                        {
                            RenderManager.SetDeselected(SelectionManager.SelectedIDs);
                            SelectionManager.ClearSelection();
                        }

                        var entity = _entityProvider.GetEntityOrDefault(id);
                        if (entity != null)
                        {
                            RenderManager.SetSelected(id.Yield());
                            SelectionManager.Select(id);
                        }
                        else
                        {
                            RenderManager.SetDeselected(SelectionManager.SelectedIDs);
                            SelectionManager.ClearSelection();
                        }
                        
                        EntitySelectionChanged?.Invoke(this, new EntitiesEventArgs(SelectionManager.SelectedEntities));
                        Invalidate();
                    }
                }
                else if (!isMultiSelect)
                {
                    RenderManager.SetDeselected(SelectionManager.SelectedIDs);
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
                var id = RenderManager.GetEntityIDFromSelection(_currentMouseLocation.ToVector2());
                SelectionManager.SelectionType = SelectionRenderer.GetSelectionTypeFromID(id);
            }));
        }

        public void StartDrag(Point location)
        {
            // Clear out any previous inputs, since they could have been from a while ago when the control last captured input
            _inputProvider.Clear();

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
            return _inputProvider.IsDown(GLFWBindings.Inputs.MouseButtons.Left) || _inputProvider.IsDown(GLFWBindings.Inputs.MouseButtons.Right);
        }

        public void EndDrag()
        {
            IsDragging = false;
            _inputProvider.Clear();
            SelectionManager.SelectionType = SelectionTypes.None;
            _pollTimer.Stop();
        }

        public void Duplicate(int entityID, int duplicateID) =>
            Invoke(new Action(() => RenderManager.Duplicate(entityID, duplicateID)));

        private void HandleInput()
        {
            if (_isDuplicating && !_inputProvider.IsDown(GLFWBindings.Inputs.Keys.LeftShift))
            {
                _isDuplicating = false;
            }

            if (_inputProvider.IsReleased(GLFWBindings.Inputs.MouseButtons.Left))
            {
                SelectionManager.SelectionType = SelectionTypes.None;
            }

            if (_inputProvider.IsDown(GLFWBindings.Inputs.MouseButtons.Left) && _inputProvider.IsDown(GLFWBindings.Inputs.MouseButtons.Right))
            {
                _panelCamera.Strafe(_inputProvider.MouseDelta);

                //IsCursorVisible = false;
                SelectionManager.SelectionType = SelectionTypes.None;
                _invalidated = true;
            }
            else if (SelectionManager.SelectionType != SelectionTypes.None)
            {
                if (_inputProvider.IsDown(GLFWBindings.Inputs.MouseButtons.Left))
                {
                    if (!_isDuplicating && _inputProvider.IsDown(GLFWBindings.Inputs.Keys.LeftShift))
                    {
                        _isDuplicating = true;

                        /*foreach (var duplication in SelectionManager.DuplicateSelection())
                        {
                            EntityDuplicated?.Invoke(this, new DuplicationEventArgs(duplication));
                        }*
                    }

                    SelectionManager.HandleEntityTransforms(TransformMode, _inputProvider.MouseDelta);

                    Invalidate();
                    Invoke(new Action(() => EntitySelectionChanged?.Invoke(this, new EntitiesEventArgs(SelectionManager.SelectedEntities))));
                }
            }
            else if (_panelCamera.ViewType == ViewTypes.Perspective)
            {
                if (_inputProvider.IsDown(GLFWBindings.Inputs.MouseButtons.Left))
                {
                    _panelCamera.Travel(_inputProvider.MouseDelta);
                    _invalidated = true;
                }
                else if (_inputProvider.IsDown(GLFWBindings.Inputs.MouseButtons.Right))
                {
                    _panelCamera.Turn(_inputProvider.MouseDelta);
                    _invalidated = true;
                }
                else if (_inputProvider.IsDown(GLFWBindings.Inputs.MouseButtons.Button1))
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
    }*/
}
