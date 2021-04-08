using PropertyChanged;
using SauceEditor.Views.GamePanels;
using SpiceEngine.Game;
using SpiceEngine.Maps;
using SpiceEngine.Rendering;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Game.Loading;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Maps;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ViewTypes = SpiceEngine.Game.ViewTypes;

namespace SauceEditor.ViewModels
{
    public class GamePaneViewModel : ViewModel
    {
        public const double MOUSE_HOLD_MILLISECONDS = 200;

        private Point _cursorLocation;
        private Timer _mouseDragTimer = new Timer(MOUSE_HOLD_MILLISECONDS);
        private bool _isMouseInWindow = false;
        private bool _isDragging = false;

        public IDragPosition DragPositioner { get; set; }
        public IEntityProvider EntityProvider { get; set; }
        public IGameLoader GameLoader { get; set; }
        public IMapper Mapper { get; set; }

        public Viewport Viewport { get; set; }

        [DoNotCheckEquality]
        public ViewTypes ViewType { get; set; }
        public string Title { get; set; }
        public float WireframeThickness { get; set; }
        public float SelectedWireframeThickness { get; set; }
        public float SelecteLightdWireframeThickness { get; set; }
        public float GridThickness { get; set; }
        public float GridUnit { get; set; }
        public bool ShowGrid { get; set; }
        //public List<IEntity> SelectedEntities { get; set; }

        private RelayCommand _dropCommand;
        public RelayCommand DropCommand
        {
            get => _dropCommand ?? (_dropCommand = new RelayCommand(
                p =>
                {
                    var args = p as DragEventArgs;
                    var mapEntity = GetDropData(args.Data);

                    var coordinates = DragPositioner.Position(args);
                    var placementID = Viewport.GetEntityIDFromPoint(coordinates);

                    if (placementID > 0)
                    {
                        var placementEntity = EntityProvider.GetEntity(placementID);
                        mapEntity.Position = new Vector3()
                        {
                            X = placementEntity.Position.X,
                            Y = placementEntity.Position.Y,
                            Z = placementEntity.Position.Z
                        };
                    }
                    else
                    {
                        // TODO - Default to Z = 0, then ray trace from camera to find corresponding X and Y coordinates
                    }

                    AddMapEntity(mapEntity);
                },
                p => p is DragEventArgs args && (args.Data.GetDataPresent(typeof(MapCamera))
                    || args.Data.GetDataPresent(typeof(MapBrush))
                    || args.Data.GetDataPresent(typeof(MapActor))
                    || args.Data.GetDataPresent(typeof(MapLight))
                    || args.Data.GetDataPresent(typeof(MapVolume)))
            ));
        }

        public GamePaneViewModel() => _mouseDragTimer.Elapsed += MouseDragTimer_Elapsed;

        public void OnShowGridChanged()
        {
            if (Viewport != null)
            {
                Viewport.RenderGrid = ShowGrid;
            }
        }

        public void OnSelectedEntitiesChanged()
        {

        }

        public void OnViewportChanged()
        {
            Viewport.MouseWheel += (s, args) => Viewport.Zoom(args.Delta);
            Viewport.MouseEnter += (s, args) => _isMouseInWindow = true;
            Viewport.MouseLeave += (s, args) => _isMouseInWindow = false;
            Viewport.MouseDown += Viewport_MouseDown;
            Viewport.MouseUp += Viewport_MouseUp;
            Viewport.MouseMove += Viewport_MouseMove;
            Viewport.PanelLoaded += (s, args) => ShowGrid = true;
            //Panel.EntitySelectionChanged += (s, args) => SelectedEntities = args.Entities;

            // Default to wireframe rendering
            Viewport.RenderMode = RenderModes.Wireframe;
            //CommandManager.InvalidateRequerySuggested();
        }

        public void OnViewTypeChanged()
        {
            Viewport.ViewType = ViewType;

            switch (ViewType)
            {
                case ViewTypes.Perspective:
                    Title = "Perspective";
                    break;
                case ViewTypes.X:
                    Title = "X";
                    break;
                case ViewTypes.Y:
                    Title = "Y";
                    break;
                case ViewTypes.Z:
                    Title = "Z";
                    break;
                default:
                    throw new ArgumentException("Could not handle ViewType " + ViewType);
            }

            Viewport.Name = Title;
        }

        private IMapEntity GetDropData(IDataObject dataObject)
        {
            if (dataObject.GetDataPresent(typeof(MapCamera)))
            {
                return dataObject.GetData(typeof(MapCamera)) as MapCamera;
            }
            else if (dataObject.GetDataPresent(typeof(MapBrush)))
            {
                return dataObject.GetData(typeof(MapBrush)) as MapBrush;
            }
            else if (dataObject.GetDataPresent(typeof(MapActor)))
            {
                return dataObject.GetData(typeof(MapActor)) as MapActor;
            }
            else if (dataObject.GetDataPresent(typeof(MapLight)))
            {
                return dataObject.GetData(typeof(MapLight)) as MapLight;
            }
            else if (dataObject.GetDataPresent(typeof(MapVolume)))
            {
                return dataObject.GetData(typeof(MapVolume)) as MapVolume;
            }
            else
            {
                return null;
            }
        }

        private void AddMapEntity(IMapEntity mapEntity)
        {
            switch (mapEntity)
            {
                case MapCamera mapCamera:
                    Mapper.AddMapCamera(mapCamera);
                    break;
                case MapBrush mapBrush:
                    Mapper.AddMapBrush(mapBrush);
                    break;
                case MapActor mapActor:
                    Mapper.AddMapActor(mapActor);
                    break;
                case MapLight mapLight:
                    Mapper.AddMapLight(mapLight);
                    break;
                case MapVolume mapVolume:
                    Mapper.AddMapVolume(mapVolume);
                    break;
            }
        }

        private void Viewport_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Viewport.IsLoaded && _isMouseInWindow)
            {
                switch (e.ChangedButton)
                {
                    case MouseButton.Left:
                        if (Mouse.RightButton == MouseButtonState.Pressed)
                        {
                            _mouseDragTimer.Stop();

                            if (!_isDragging)
                            {
                                BeginDrag();
                            }
                        }
                        else
                        {
                            _mouseDragTimer.Start();
                        }
                        break;
                    case MouseButton.Right:
                        _mouseDragTimer.Stop();

                        if (!_isDragging)
                        {
                            BeginDrag();
                        }
                        break;
                    case MouseButton.XButton1:
                        _mouseDragTimer.Stop();

                        if (!_isDragging)
                        {
                            BeginDrag();
                        }
                        break;
                }
            }
        }

        private void Viewport_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_isDragging)
            {
                _isDragging = false;
                Mouse.Capture(null);
                Viewport.Cursor = Cursors.Arrow;

                if (Mouse.LeftButton == MouseButtonState.Released && Mouse.RightButton == MouseButtonState.Released && Mouse.XButton1 == MouseButtonState.Released)
                {
                    // Double-check with Panel, since its input tracking is more reliable
                    if (!Viewport.InputManager.IsDown(SpiceEngine.GLFWBindings.Inputs.MouseButtons.Left) && !Viewport.InputManager.IsDown(SpiceEngine.GLFWBindings.Inputs.MouseButtons.Right) && !Viewport.InputManager.IsDown(SpiceEngine.GLFWBindings.Inputs.MouseButtons.Button4))
                    {
                        _mouseDragTimer.Stop();

                        if (_isDragging)
                        {
                            EndDrag();
                        }
                        else if (Viewport.IsLoaded && e.ChangedButton == MouseButton.Left)
                        {
                            Viewport.SelectEntity(e.GetPosition(Viewport), Keyboard.IsKeyDown(Key.LeftCtrl));
                        }
                    }
                }
            }
        }

        private void Viewport_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _cursorLocation = Mouse.GetPosition(Viewport);

            if (_isDragging && _isMouseInWindow)
            {
                //Mouse.SetPosition(_startMouseLocation.X, _startMouseLocation.Y);
                Mouse.Capture(Viewport);
                Viewport.Cursor = Cursors.None;
            }
        }

        private void BeginDrag()
        {
            _isDragging = true;

            _cursorLocation = Mouse.GetPosition(Viewport);
            Viewport.Cursor = Cursors.None;
            Mouse.Capture(Viewport);

            _mouseDragTimer.Start();
        }

        private void EndDrag()
        {
            _isDragging = false;
            _mouseDragTimer.Stop();

            Mouse.Capture(null);
            Viewport.Cursor = Cursors.Arrow;
            //System.Windows.Forms.Cursor.Position = _cursorLocation;
        }

        private void MouseDragTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _mouseDragTimer.Stop();
            //Application.Current.Dispatcher.Invoke(() =>
            //{
                if (_isDragging)
                {
                    Viewport.SetSelectionType();
                    Viewport.BeginUpdates();
                }
            //});
        }
    }
}