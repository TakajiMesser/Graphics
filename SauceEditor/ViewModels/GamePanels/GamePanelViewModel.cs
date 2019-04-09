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

        private GamePanel _gamePanel;
        private string _title;

        private ViewTypes _viewType;
        private System.Drawing.Point _cursorLocation;
        private Timer _mouseHoldtimer = new Timer(MOUSE_HOLD_MILLISECONDS);

        private RelayCommand _wireframeCommand;
        private RelayCommand _diffuseCommand;
        private RelayCommand _litCommand;

        private float _wireframeThickness;
        private float _selectedWireframeThickness;
        private float _selectedLightWireframeThickness;
        private float _gridthickness;
        private float _gridUnit;

        private bool _showGrid;

        public GamePanel Panel => _gamePanel;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public RelayCommand WireframeCommand
        {
            get
            {
                if (_wireframeCommand == null)
                {
                    _wireframeCommand = new RelayCommand(
                        p =>
                        {
                            _gamePanel.RenderMode = RenderModes.Wireframe;
                            _gamePanel.Invalidate();
                            CommandManager.InvalidateRequerySuggested();
                        },
                        p => _gamePanel != null && _gamePanel.RenderMode != RenderModes.Wireframe
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
                            _gamePanel.RenderMode = RenderModes.Diffuse;
                            _gamePanel.Invalidate();
                            CommandManager.InvalidateRequerySuggested();
                        },
                        p => _gamePanel != null && _gamePanel.RenderMode != RenderModes.Diffuse
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
                            _gamePanel.RenderMode = RenderModes.Lit;
                            _gamePanel.Invalidate();
                            CommandManager.InvalidateRequerySuggested();
                        },
                        p => _gamePanel != null && _gamePanel.RenderMode != RenderModes.Lit
                    );
                }

                return _litCommand;
            }
        }

        public float WireframeThickness
        {
            get => _wireframeThickness;
            set => SetProperty(ref _wireframeThickness, value);
        }

        public float SelectedWireframeThickness
        {
            get => _selectedWireframeThickness;
            set => SetProperty(ref _selectedWireframeThickness, value);
        }

        public float SelecteLightdWireframeThickness
        {
            get => _selectedLightWireframeThickness;
            set => SetProperty(ref _selectedLightWireframeThickness, value);
        }

        public float GridThickness
        {
            get => _gridthickness;
            set => SetProperty(ref _gridthickness, value);
        }

        public float GridUnit
        {
            get => _gridUnit;
            set => SetProperty(ref _gridUnit, value);
        }

        public bool ShowGrid
        {
            get => _showGrid;
            set
            {
                if (_gamePanel != null)
                {
                    _gamePanel.RenderGrid = value;
                }

                SetProperty(ref _showGrid, value);
            }
        }

        public GamePanelViewModel()
        {
            _mouseHoldtimer.Elapsed += MouseHoldtimer_Elapsed;
        }

        public void SetGamePanel(GamePanel panel)
        {
            _gamePanel = panel;
            _gamePanel.MouseWheel += (s, args) => _gamePanel.Zoom(args.Delta);
            _gamePanel.MouseDown += Panel_MouseDown;
            _gamePanel.MouseUp += Panel_MouseUp;
            _gamePanel.PanelLoaded += (s, args) => ShowGrid = true;
            _gamePanel.ChangeCursorVisibility += GamePanel_ChangeCursorVisibility;
            _gamePanel.SetViewType(_viewType);

            // Default to wireframe rendering
            _gamePanel.RenderMode = RenderModes.Wireframe;
            CommandManager.InvalidateRequerySuggested();
        }

        public void SetViewType(ViewTypes viewType)
        {
            _viewType = viewType;

            switch (viewType)
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
                    throw new ArgumentException("Could not handle ViewType " + viewType);
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
            if (_gamePanel.IsLoaded)
            {
                _cursorLocation = System.Windows.Forms.Cursor.Position;
                System.Windows.Forms.Cursor.Hide();
                _gamePanel.Capture = true;
                //Mouse.Capture(PanelHost);
                _gamePanel.StartDrag(_cursorLocation);
            }
        }

        private void EndDrag()
        {
            _gamePanel.Capture = false; //PanelHost.ReleaseMouseCapture();
            System.Windows.Forms.Cursor.Show();
            System.Windows.Forms.Cursor.Position = _cursorLocation;
            _gamePanel.EndDrag();
        }

        private void Panel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //Panel.Capture = true;//.Capture(PanelHost);

            switch (e.Button)
            {
                case System.Windows.Forms.MouseButtons.Left:
                    if (Mouse.RightButton == MouseButtonState.Pressed)
                    {
                        if (!_gamePanel.IsDragging)
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

                    if (!_gamePanel.IsDragging)
                    {
                        BeginDrag();
                    }
                    break;
                case System.Windows.Forms.MouseButtons.XButton1:
                    _mouseHoldtimer.Stop();

                    if (!_gamePanel.IsDragging)
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
                if (!_gamePanel.IsDragging)
                {
                    _gamePanel.SetSelectionType();
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
                if (!_gamePanel.IsHeld())
                {
                    _mouseHoldtimer.Stop();

                    if (_gamePanel.IsDragging)
                    {
                        EndDrag();
                    }
                    else if (_gamePanel.IsLoaded && e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        var point = e.Location;
                        _gamePanel.SelectEntity(point, Keyboard.IsKeyDown(Key.LeftCtrl));
                    }
                }
            }
        }
    }
}