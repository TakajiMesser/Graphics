using OpenTK;
using SauceEditor.Models;
using SauceEditor.Utilities;
using SpiceEngine.Entities;
using SpiceEngine.Game;
using SpiceEngine.Maps;
using SpiceEngine.Rendering;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ViewTypes = SpiceEngine.Game.ViewTypes;

namespace SauceEditor.ViewModels
{
    public class GamePanelViewModel : ViewModel
    {
        public const double MOUSE_HOLD_MILLISECONDS = 200;

        private ViewTypes _viewType;
        private System.Drawing.Point _cursorLocation;
        private Timer _mouseHoldtimer = new Timer(MOUSE_HOLD_MILLISECONDS);

        private RelayCommand _wireframeCommand;
        private RelayCommand _diffuseCommand;
        private RelayCommand _litCommand;

        public RelayCommand WireframeCommand
        {
            get
            {
                if (_wireframeCommand == null)
                {
                    _wireframeCommand = new RelayCommand(
                        p =>
                        {
                            Panel.RenderMode = RenderModes.Wireframe;
                            Panel.Invalidate();
                            CommandManager.InvalidateRequerySuggested();
                        },
                        p => Panel != null && Panel.RenderMode != RenderModes.Wireframe
                    );
                }

                return _wireframeCommand;
            }
        }

        public RelayCommand DiffuseCommand
        {
            get
            {
                if (_diffuseCommand == null)
                {
                    _diffuseCommand = new RelayCommand(
                        p =>
                        {
                            Panel.RenderMode = RenderModes.Diffuse;
                            Panel.Invalidate();
                            CommandManager.InvalidateRequerySuggested();
                        },
                        p => Panel != null && Panel.RenderMode != RenderModes.Diffuse
                    );
                }

                return _diffuseCommand;
            }
        }

        public RelayCommand LitCommand
        {
            get
            {
                if (_litCommand == null)
                {
                    _litCommand = new RelayCommand(
                        p =>
                        {
                            Panel.RenderMode = RenderModes.Lit;
                            Panel.Invalidate();
                            CommandManager.InvalidateRequerySuggested();
                        },
                        p => Panel != null && Panel.RenderMode != RenderModes.Lit
                    );
                }

                return _litCommand;
            }
        }

        public GamePanel Panel { get; set; }
        public ViewTypes ViewType { get; set; }
        public string Title { get; set; }
        public float WireframeThickness { get; set; }
        public float SelectedWireframeThickness { get; set; }
        public float SelecteLightdWireframeThickness { get; set; }
        public float GridThickness { get; set; }
        public float GridUnit { get; set; }
        public bool ShowGrid { get; set; }

        public void OnShowGridChanged()
        {
            if (Panel != null)
            {
                Panel.RenderGrid = ShowGrid;
            }
        }

        public GamePanelViewModel()
        {
            _mouseHoldtimer.Elapsed += MouseHoldtimer_Elapsed;
        }

        public void OnPanelChanged()
        {
            Panel.MouseWheel += (s, args) => Panel.Zoom(args.Delta);
            Panel.MouseDown += Panel_MouseDown;
            Panel.MouseUp += Panel_MouseUp;
            Panel.PanelLoaded += (s, args) => ShowGrid = true;
            Panel.ChangeCursorVisibility += GamePanel_ChangeCursorVisibility;

            // Default to wireframe rendering
            Panel.RenderMode = RenderModes.Wireframe;
            CommandManager.InvalidateRequerySuggested();
        }

        public void OnViewTypeChanged()
        {
            Panel.SetViewType(_viewType);

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

        private void GamePanel_ChangeCursorVisibility(object sender, CursorEventArgs e)
        {
            if (e.ShowCursor)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    //System.Windows.Forms.Cursor.Clip = Rectangle.Empty;
                    System.Windows.Forms.Cursor.Position = _cursorLocation;
                    System.Windows.Forms.Cursor.Show();
                    //Mouse.Capture(_previousCapture);
                });
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    //var bounds = RenderTransform.TransformBounds(new Rect(RenderSize));
                    //System.Windows.Forms.Cursor.Clip = new Rectangle((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height);
                    //System.Windows.Forms.Cursor.Clip = new Rectangle((int)Top, (int)Left, (int)Width, (int)Height);
                    //Visibility = Visibility.Visible;
                    //IsEnabled = true;

                    _cursorLocation = System.Windows.Forms.Cursor.Position;
                    System.Windows.Forms.Cursor.Hide();
                });
            }
        }

        private void BeginDrag()
        {
            if (Panel.IsLoaded)
            {
                _cursorLocation = System.Windows.Forms.Cursor.Position;
                System.Windows.Forms.Cursor.Hide();
                Panel.Capture = true;
                //Mouse.Capture(PanelHost);
                Panel.StartDrag(_cursorLocation);
            }
        }

        private void EndDrag()
        {
            Panel.Capture = false; //PanelHost.ReleaseMouseCapture();
            System.Windows.Forms.Cursor.Show();
            System.Windows.Forms.Cursor.Position = _cursorLocation;
            Panel.EndDrag();
        }

        private void Panel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //Panel.Capture = true;//.Capture(PanelHost);

            switch (e.Button)
            {
                case System.Windows.Forms.MouseButtons.Left:
                    if (Mouse.RightButton == MouseButtonState.Pressed)
                    {
                        if (!Panel.IsDragging)
                        {
                            BeginDrag();
                        }
                    }
                    else
                    {
                        _mouseHoldtimer.Start();
                    }
                    break;
                case System.Windows.Forms.MouseButtons.Right:
                    _mouseHoldtimer.Stop();

                    if (!Panel.IsDragging)
                    {
                        BeginDrag();
                    }
                    break;
                case System.Windows.Forms.MouseButtons.XButton1:
                    _mouseHoldtimer.Stop();

                    if (!Panel.IsDragging)
                    {
                        BeginDrag();
                    }
                    break;
            }
        }

        private void MouseHoldtimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _mouseHoldtimer.Stop();

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (!Panel.IsDragging)
                {
                    Panel.SetSelectionType();
                    BeginDrag();
                }
            });
        }

        private void Panel_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //System.Windows.Forms.Cursor.Position = _cursorLocation;

            if (Mouse.LeftButton == MouseButtonState.Released && Mouse.RightButton == MouseButtonState.Released && Mouse.XButton1 == MouseButtonState.Released)
            {
                // Double-check with Panel, since its input tracking is more reliable
                if (!Panel.IsHeld())
                {
                    _mouseHoldtimer.Stop();

                    if (Panel.IsDragging)
                    {
                        EndDrag();
                    }
                    else if (Panel.IsLoaded && e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        var point = e.Location;
                        Panel.SelectEntity(point, Keyboard.IsKeyDown(Key.LeftCtrl));
                    }
                }
            }
        }
    }
}