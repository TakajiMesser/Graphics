using SpiceEngine.Entities.Selection;
using SpiceEngine.HID;
using SpiceEngine.Rendering;
using SpiceEngine.Utilities;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Game;
using SpiceEngineCore.Game.Settings;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Maps;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Utilities;
using SweetGraphicsCore.Renderers.Processing;
using SweetGraphicsCore.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using TangyHIDCore.Inputs;
using InputManager = TangyHIDCore.Inputs.InputManager;

namespace SpiceEngine.Game
{
    /// <summary>
    /// Interaction logic for Viewport.xaml
    /// </summary>
    public partial class Viewport : UserControl, IInputTracker, IInvoker
    {
        public const double UPDATE_MILLISECONDS = 16.67;

        private Point _currentMouseLocation;
        private Point _startMouseLocation;
        private Timer _updateTimer = new Timer(UPDATE_MILLISECONDS);

        private FrameWindow _frameWindow;
        private IRenderContext _renderContext;
        private Configuration _configuration;

        private PanelCamera _panelCamera;

        private bool _isUpdating = false;
        private bool _isInvalidated = false;
        private bool _isDuplicating = false;

        private bool _isLoaded = false;
        private bool _isStarted = false;
        private object _loadLock = new object();

        private TransformModes _transformMode;
        private RenderModes _renderMode;
        private ViewTypes _viewType;

        public Viewport()
        {
            InitializeComponent();
            
            Loaded += Viewport_Loaded;
            Unloaded += Viewport_Unloaded;
            SizeChanged += (s, args) => WindowSize.Update((int)args.NewSize.Width, (int)args.NewSize.Height);
            WindowSize = new Resolution((int)Width, (int)Height);
        }

        public Resolution WindowSize { get; private set; }

        // TODO - Abstract out skybox texture provider to avoid needing map here
        public IMap Map { get; set; }
        public SimulationManager SimulationManager { get; set; }
        public EditorRenderManager RenderManager { get; private set; }
        public SelectionManager SelectionManager { get; private set; }
        public InputManager InputManager { get; private set; }

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
                        Invalidate();
                    }
                }
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

        public event EventHandler<CursorEventArgs> CursorPositionChanged;
        public event EventHandler<CursorEventArgs> Scrolled;
        public event EventHandler CursorEntered;
        public event EventHandler CursorExited;
        public event EventHandler<KeyEventArgs> KeyAction;
        public event EventHandler<KeyEventArgs> KeyPress;
        public event EventHandler<KeyEventArgs> KeyRelease;
        public event EventHandler<MouseButtonEventArgs> MouseButton;

        public void Initialize(Configuration configuration, SimulationManager simulationManager, IMap map)
        {
            _frameWindow = new FrameWindow(configuration, WindowContextFactory.SharedInstance, Frame);
            _configuration = configuration;

            SizeChanged += Viewport_SizeChanged;

            _updateTimer.Elapsed += UpdateTimer_Elapsed;

            Map = map;
            SelectionManager = new SelectionManager(simulationManager.EntityProvider);

            SetUpCallbacks();
            InputManager = new InputManager();
            InputManager.RegisterDevices(this);

            lock (_loadLock)
            {
                SimulationManager = simulationManager;

                if (_isLoaded && !_isStarted)
                {
                    _isStarted = true;
                    StartFrameWindow();
                }
            }
        }

        private void SetUpCallbacks()
        {
            MouseMove += (s, args) =>  
            {
                var position = args.GetPosition(this);
                CursorPositionChanged?.Invoke(s, new CursorEventArgs(position.X, position.Y));
            };

            MouseWheel += (s, args) => Scrolled?.Invoke(s, new CursorEventArgs(0.0, args.Delta));
            MouseEnter += (s, args) => CursorEntered?.Invoke(s, EventArgs.Empty);
            MouseLeave += (s, args) => CursorExited?.Invoke(s, EventArgs.Empty);

            KeyDown += (s, args) =>
            {
                var key = args.Key.ToGLFWKey();
                KeyPress?.Invoke(s, new KeyEventArgs(key, 0, GLFWBindings.Inputs.InputStates.Press, GLFWBindings.Inputs.ModifierKeys.None));
                KeyAction?.Invoke(s, new KeyEventArgs(key, 0, GLFWBindings.Inputs.InputStates.Press, GLFWBindings.Inputs.ModifierKeys.None));
            };

            KeyUp += (s, args) =>
            {
                var key = args.Key.ToGLFWKey();
                KeyRelease?.Invoke(s, new KeyEventArgs(key, 0, GLFWBindings.Inputs.InputStates.Release, GLFWBindings.Inputs.ModifierKeys.None));
                KeyAction?.Invoke(s, new KeyEventArgs(key, 0, GLFWBindings.Inputs.InputStates.Release, GLFWBindings.Inputs.ModifierKeys.None));
            };

            MouseDown += (s, args) => MouseButton?.Invoke(s, new MouseButtonEventArgs(args.ChangedButton.ToGLFWMouseButton(), GLFWBindings.Inputs.InputStates.Press, GLFWBindings.Inputs.ModifierKeys.None));
            MouseUp += (s, args) => MouseButton?.Invoke(s, new MouseButtonEventArgs(args.ChangedButton.ToGLFWMouseButton(), GLFWBindings.Inputs.InputStates.Release, GLFWBindings.Inputs.ModifierKeys.None));
        }

        private void Viewport_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            lock (_loadLock)
            {
                _isLoaded = true;

                if (SimulationManager != null && !_isStarted)
                {
                    _isStarted = true;
                    StartFrameWindow();
                }
            }
        }

        private void Viewport_Unloaded(object sender, System.Windows.RoutedEventArgs e) { }

        private void Viewport_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e) => _frameWindow?.Resize((int)e.NewSize.Width, (int)e.NewSize.Height);

        public void BeginUpdates()
        {
            InvokeSync(() =>
            {
                _isUpdating = true;
                _updateTimer.Start();
                _frameWindow.Start();
            });
        }

        public void EndUpdates()
        {
            _isUpdating = false;
            _updateTimer.Stop();
            _frameWindow.Stop();
        }

        private void UpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _updateTimer.Stop();

            HandleInput();
            InputManager.Tick();

            /*if (_isInvalidated)
            {
                Invalidate();
            }*/

            if (_isUpdating)
            {
                _updateTimer.Start();
            }
        }

        private void HandleInput()
        {
            if (_isDuplicating && !InputManager.IsDown(GLFWBindings.Inputs.Keys.LeftShift))
            {
                _isDuplicating = false;
            }

            if (InputManager.IsReleased(GLFWBindings.Inputs.MouseButtons.Left))
            {
                SelectionManager.SelectionType = SelectionTypes.None;
            }

            if (InputManager.IsDown(GLFWBindings.Inputs.MouseButtons.Left) && InputManager.IsDown(GLFWBindings.Inputs.MouseButtons.Right))
            {
                _panelCamera.Strafe(InputManager.MouseDelta);

                //IsCursorVisible = false;
                SelectionManager.SelectionType = SelectionTypes.None;
                _isInvalidated = true;
            }
            else if (SelectionManager.SelectionType != SelectionTypes.None)
            {
                if (InputManager.IsDown(GLFWBindings.Inputs.MouseButtons.Left))
                {
                    if (!_isDuplicating && InputManager.IsDown(GLFWBindings.Inputs.Keys.LeftShift))
                    {
                        _isDuplicating = true;

                        /*foreach (var duplication in SelectionManager.DuplicateSelection())
                        {
                            EntityDuplicated?.Invoke(this, new DuplicationEventArgs(duplication));
                        }*/
                    }

                    SelectionManager.HandleEntityTransforms(TransformMode, InputManager.MouseDelta);

                    Invalidate();
                    Application.Current.Dispatcher.Invoke(() => EntitySelectionChanged?.Invoke(this, new EntitiesEventArgs(SelectionManager.SelectedEntities)));
                }
            }
            else if (_panelCamera.ViewType == ViewTypes.Perspective)
            {
                if (InputManager.IsDown(GLFWBindings.Inputs.MouseButtons.Left))
                {
                    _panelCamera.Travel(InputManager.MouseDelta);
                    _isInvalidated = true;
                }
                else if (InputManager.IsDown(GLFWBindings.Inputs.MouseButtons.Right))
                {
                    _panelCamera.Turn(InputManager.MouseDelta);
                    _isInvalidated = true;
                }
                else if (InputManager.IsDown(GLFWBindings.Inputs.MouseButtons.Button1))
                {
                    if (SelectionManager.SelectionCount > 0)
                    {
                        _panelCamera.Pivot(InputManager.MouseDelta, InputManager.MouseWheelDelta, SelectionManager.Position);
                        _isInvalidated = true;
                    }
                }
            }
        }

        private void StartFrameWindow()
        {
            _panelCamera = new PanelCamera(_frameWindow.Display.Resolution, SimulationManager.EntityProvider)
            {
                ViewType = ViewType
            };

            _frameWindow.Loaded += (s, args) =>
            {
                // TODO - This is janky, but force reprocessing the load queue
                PanelLoaded?.Invoke(this, new PanelLoadedEventArgs());
            };

            // TODO - Should we run this all on the UI thread?
            InvokeSync(() =>
            {
                _renderContext = RenderContextFactory.SharedInstance.CreateRenderContext(_configuration, _frameWindow);
                _frameWindow.LoadBuffers(_renderContext);

                RenderManager = new EditorRenderManager(_renderContext, _frameWindow.Display, _panelCamera)
                {
                    Invoker = this
                };

                _frameWindow.LoadSimulation(SimulationManager, RenderManager, SelectionManager, _panelCamera, _renderMode);
                _frameWindow.StartLoad();
            });
        }

        public void CenterView()
        {
            if (SimulationManager != null && _panelCamera != null && SelectionManager.SelectionCount > 0)
            {
                _panelCamera.CenterView(SelectionManager.Position);
                Invalidate();
            }
        }

        public void SetSelectionType()
        {
            InvokeSync(() =>
            {
                var id = RenderManager.GetEntityIDFromSelection(new Vector2((float)_currentMouseLocation.X, (float)_currentMouseLocation.Y));
                SelectionManager.SelectionType = SelectionRenderer.GetSelectionTypeFromID(id);
            });
        }

        // TODO - Fix this call...
        public void AddEntity(int entityID, IRenderable renderable) { }// => RenderManager?.AddEntity(entityID, renderable);
        public void RemoveEntity(int entityID) => RenderManager?.RemoveEntity(entityID);

        // TODO - Make this method less janky and terrible
        public void DoLoad() => RenderManager?.LoadBatcher();

        public int GetEntityIDFromPoint(Point coordinates)
        {
            Invalidate();
            return RenderManager.GetEntityIDFromSelection(new Vector2((float)coordinates.X, (float)coordinates.Y));
        }

        public void SelectEntity(int id)
        {
            RenderManager?.SetSelected(id.Yield());

            SelectionManager.Select(id);
            Invalidate();
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

                        var entity = SimulationManager.EntityProvider.GetEntityOrDefault(id);
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
                var selectedEntity = SimulationManager.EntityProvider.GetEntity(entity.ID);

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

        public void ClearSelectedEntities() => SelectionManager.Clear();

        public Task InvokeAsync(Action action) => Task.Run(() =>
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                //_frameWindow.MakeCurrent();
                action();
            });
        });

        public void InvokeSync(Action action)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                //_frameWindow.MakeCurrent();
                action();
            });
        }

        public void ForceUpdate() => Invalidate();

        public void Invalidate()
        {
            InvokeSync(() =>
            {
                _frameWindow.ForceUpdate();
            });

            /*_isInvalidated = false;

            // TODO - Determine how to handle this
            if (SelectionManager.SelectionCount > 0)
            {
                // This is still necessary right now for rendering lights and transform arrows...
                RenderManager.RenderSelection(SelectionManager.SelectedEntities, TransformMode);
            }*/
        }
    }
}
