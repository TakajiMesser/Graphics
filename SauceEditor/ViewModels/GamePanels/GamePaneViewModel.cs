using SauceEditor.Views.GamePanels;
using SauceEditorCore.Models.Components;
using SpiceEngine.Game;
using SpiceEngine.Rendering;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Game.Loading;
using SpiceEngineCore.Rendering.Models;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using ViewTypes = SpiceEngine.Game.ViewTypes;

namespace SauceEditor.ViewModels
{
    public class GamePaneViewModel : ViewModel
    {
        public const double MOUSE_HOLD_MILLISECONDS = 200;

        private System.Drawing.Point _cursorLocation;
        private Timer _mouseHoldTimer = new Timer(MOUSE_HOLD_MILLISECONDS);

        public IPosition Positioner { get; set; }
        public IEntityProvider EntityProvider { get; set; }
        public IGameLoader GameLoader { get; set; }
        public IMapper Mapper { get; set; }

        public GameControl Control { get; set; }
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
                    var args = (DragEventArgs)p;

                    Console.WriteLine("DEM ARGS = {");

                    Console.WriteLine("}");

                    if (args.Data.GetDataPresent(typeof(ModelMesh)))
                    {
                        // TODO - We also need to get the drop position relative to the game view so we know where to place the object
                        var meshShape = args.Data.GetData(typeof(ModelMesh)) as ModelMesh;
                        Positioner?.Position(meshShape, args);
                    }
                },
                p => true
            ));
        }

        public GamePaneViewModel()
        {
            _mouseHoldTimer.Elapsed += MouseHoldTimer_Elapsed;
        }

        public void OnShowGridChanged()
        {
            if (Control != null)
            {
                Control.RenderGrid = ShowGrid;
            }
        }

        public void OnSelectedEntitiesChanged()
        {

        }

        public void OnControlChanged()
        {
            Control.MouseWheel += (s, args) => Control.Zoom(args.Delta);
            Control.MouseDown += Panel_MouseDown;
            Control.MouseUp += Panel_MouseUp;
            Control.PanelLoaded += (s, args) => ShowGrid = true;
            //Panel.EntitySelectionChanged += (s, args) => SelectedEntities = args.Entities;

            // Default to wireframe rendering
            Control.RenderMode = RenderModes.Wireframe;
            //CommandManager.InvalidateRequerySuggested();
        }

        public void OnViewTypeChanged()
        {
            Control.ViewType = ViewType;

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
        }

        private void BeginDrag()
        {
            if (Control.IsLoaded)
            {
                _cursorLocation = System.Windows.Forms.Cursor.Position;
                System.Windows.Forms.Cursor.Hide();
                Control.Capture = true;
                //Mouse.Capture(PanelHost);
                Control.StartDrag(_cursorLocation);
            }
        }

        private void EndDrag()
        {
            Control.Capture = false; //PanelHost.ReleaseMouseCapture();
            System.Windows.Forms.Cursor.Show();
            System.Windows.Forms.Cursor.Position = _cursorLocation;
            Control.EndDrag();
        }

        private void MouseHoldTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _mouseHoldTimer.Stop();

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (!Control.IsDragging)
                {
                    Control.SetSelectionType();
                    BeginDrag();
                }
            });
        }

        private void Panel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //Panel.Capture = true;//.Capture(PanelHost);

            switch (e.Button)
            {
                case System.Windows.Forms.MouseButtons.Left:
                    if (Mouse.RightButton == MouseButtonState.Pressed)
                    {
                        if (!Control.IsDragging)
                        {
                            BeginDrag();
                        }
                    }
                    else
                    {
                        _mouseHoldTimer.Start();
                    }
                    break;
                case System.Windows.Forms.MouseButtons.Right:
                    _mouseHoldTimer.Stop();

                    if (!Control.IsDragging)
                    {
                        BeginDrag();
                    }
                    break;
                case System.Windows.Forms.MouseButtons.XButton1:
                    _mouseHoldTimer.Stop();

                    if (!Control.IsDragging)
                    {
                        BeginDrag();
                    }
                    break;
            }
        }

        private void Panel_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //System.Windows.Forms.Cursor.Position = _cursorLocation;

            if (Mouse.LeftButton == MouseButtonState.Released && Mouse.RightButton == MouseButtonState.Released && Mouse.XButton1 == MouseButtonState.Released)
            {
                // Double-check with Panel, since its input tracking is more reliable
                if (!Control.IsHeld())
                {
                    _mouseHoldTimer.Stop();

                    if (Control.IsDragging)
                    {
                        EndDrag();
                    }
                    else if (Control.IsLoaded && e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        var point = e.Location;
                        Control.SelectEntity(point, Keyboard.IsKeyDown(Key.LeftCtrl));
                    }
                }
            }
        }
    }
}