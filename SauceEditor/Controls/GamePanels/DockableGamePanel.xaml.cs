using DockingLibrary;
using Jidai.GameObjects;
using OpenTK;
using SauceEditor.Commands;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using TakoEngine.Entities.Cameras;
using TakoEngine.Game;
using TakoEngine.Maps;
using TakoEngine.Rendering.Processing;
using Camera = TakoEngine.Entities.Cameras.Camera;

namespace SauceEditor.Controls.GamePanels
{
    /// <summary>
    /// Interaction logic for DockableGamePanel.xaml
    /// </summary>
    public partial class DockableGamePanel : DockableContent
    {
        public const double MOUSE_HOLD_MILLISECONDS = 500;

        public ViewTypes ViewType => Panel.ViewType;

        public event EventHandler<CommandEventArgs> CommandExecuted;
        public event EventHandler<EntitySelectedEventArgs> EntitySelectionChanged;

        private System.Drawing.Point _cursorLocation;
        private Timer _mouseHoldtimer = new Timer(MOUSE_HOLD_MILLISECONDS);

        private bool _leftMouseButtonDown = false;
        private bool _rightMouseButtonDown = false;

        public DockableGamePanel() => InitializeComponent();
        public DockableGamePanel(DockManager dockManager, ViewTypes viewType/*, GameState gameState*/) : base(dockManager)
        {
            InitializeComponent();

            _mouseHoldtimer.Elapsed += MouseHoldtimer_Elapsed;
            Panel.ViewType = viewType;

            Panel.EntitySelectionChanged += (s, args) => EntitySelectionChanged?.Invoke(this, args);
            //Panel.TransformModeChanged += GamePanel_TransformModeChanged;
            Panel.ChangeCursorVisibility += GamePanel_ChangeCursorVisibility;

            Panel.MouseWheel += (s, args) => Panel.Zoom(args.Delta);
            Panel.MouseDown += Panel_MouseDown;
            Panel.MouseUp += Panel_MouseUp;
            //Panel.MouseMove += Panel_MouseMove;

            // Default to wireframe rendering
            WireframeButton.IsEnabled = false;
            Panel.RenderMode = RenderModes.Wireframe;
        }

        private void Panel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Panel.Capture = true;//.Capture(PanelHost);

            switch (e.Button)
            {
                case System.Windows.Forms.MouseButtons.Left:
                    _leftMouseButtonDown = true;

                    if (_rightMouseButtonDown)
                    {
                        if (!Panel.IsDragging)
                        {
                            _cursorLocation = System.Windows.Forms.Cursor.Position;
                            System.Windows.Forms.Cursor.Hide();
                            Panel.StartDrag();
                        }
                    }
                    else
                    {
                        _mouseHoldtimer.Start();
                    }
                    break;
                case System.Windows.Forms.MouseButtons.Right:
                    _rightMouseButtonDown = true;
                    _mouseHoldtimer.Stop();

                    if (!Panel.IsDragging)
                    {
                        _cursorLocation = System.Windows.Forms.Cursor.Position;
                        System.Windows.Forms.Cursor.Hide();
                        Panel.StartDrag();
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
                    //Panel.Capture = true;
                    //System.Windows.Forms.Cursor.Clip = Rectangle.Empty;
                    _cursorLocation = System.Windows.Forms.Cursor.Position;
                    System.Windows.Forms.Cursor.Hide();
                    Panel.Capture = true;
                    //Mouse.Capture(PanelHost);
                    Panel.StartDrag();
                }
                //Mouse.Capture(_previousCapture);
            });
        }

        //private object _mouseLock = new object();

        private void Panel_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            switch (e.Button)
            {
                case System.Windows.Forms.MouseButtons.Left:
                    _leftMouseButtonDown = false;
                    break;
                case System.Windows.Forms.MouseButtons.Right:
                    _rightMouseButtonDown = false;
                    break;
            }

            //if (!_leftMouseButtonDown && !_rightMouseButtonDown)
            if (Mouse.LeftButton == MouseButtonState.Released && Mouse.RightButton == MouseButtonState.Released)
            {
                _mouseHoldtimer.Stop();
                _leftMouseButtonDown = false;
                _rightMouseButtonDown = false;

                //lock (_mouseLock)
                //{
                    if (Panel.IsDragging)
                    {
                        Panel.Capture = false; //PanelHost.ReleaseMouseCapture();
                        System.Windows.Forms.Cursor.Show();
                        System.Windows.Forms.Cursor.Position = _cursorLocation;
                        Panel.EndDrag();
                    }
                    else if (Panel.IsLoaded)
                    {
                        var point = e.Location;
                        Panel.SelectEntity(point);
                    }
                //}
            }
        }

        /*private void PanelHost_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _timer.Start();
            _held = false;

            _timer.Elapsed += (s, args) =>
            {
                _held = true;

                if (PanelHost.CaptureMouse())
                {

                }
            };
        }

        private void PanelHost_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _timer.Stop();
            PanelHost.ReleaseMouseCapture();

            if (!_held)
            {
                var point = e.GetPosition(PanelHost);
                Panel.SelectEntity(point);
            }
        }*/

        /*public void LoadGameState(GameState gameState, Map map)
        {
            Panel.LoadGameState(gameState, map);
        }*/

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

        /*private void GamePanel_TransformModeChanged(object sender, TransformModeEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                switch (e.TransformMode)
                {
                    case TransformModes.Translate:
                        TranslateButton.IsEnabled = false;
                        RotateButton.IsEnabled = true;
                        ScaleButton.IsEnabled = true;
                        break;

                    case TransformModes.Rotate:
                        TranslateButton.IsEnabled = true;
                        RotateButton.IsEnabled = false;
                        ScaleButton.IsEnabled = true;
                        break;

                    case TransformModes.Scale:
                        TranslateButton.IsEnabled = true;
                        RotateButton.IsEnabled = true;
                        ScaleButton.IsEnabled = false;
                        break;
                }
            });
        }*/

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            e.Handled = true;
            //base.OnMouseUp(e);
        }

        protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
        {
            if (!Panel.IsCursorVisible || Panel.IsCameraMoving)
            {
                System.Windows.Forms.Cursor.Position = _cursorLocation;
            }

            e.Handled = true;
            //base.OnMouseLeave(e);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            //DockManager.ParentWindow = this;
            //Grid.Children.Add(GameWindow);
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            //GamePanel?.Close();
            base.OnClosing(e);
        }

        private void WireframeButton_Click(object sender, RoutedEventArgs e)
        {
            WireframeButton.IsEnabled = false;
            DiffuseButton.IsEnabled = true;
            LitButton.IsEnabled = true;
            Panel.RenderMode = RenderModes.Wireframe;
            Panel.Invalidate();

            /*var action = new Action(() =>
            {
                WireframeButton.IsEnabled = false;
                DiffuseButton.IsEnabled = true;
                LitButton.IsEnabled = true;
                Panel.RenderMode = RenderModes.Wireframe;
                Panel.Invalidate();
            });

            CommandExecuted?.Invoke(this, new CommandEventArgs(new Command(this, action)));*/
        }

        private void DiffuseButton_Click(object sender, RoutedEventArgs e)
        {
            WireframeButton.IsEnabled = true;
            DiffuseButton.IsEnabled = false;
            LitButton.IsEnabled = true;
            Panel.RenderMode = RenderModes.Diffuse;
            Panel.Invalidate();
        }

        private void LitButton_Click(object sender, RoutedEventArgs e)
        {
            WireframeButton.IsEnabled = true;
            DiffuseButton.IsEnabled = true;
            LitButton.IsEnabled = false;
            Panel.RenderMode = RenderModes.Lit;
            Panel.Invalidate();
        }
    }
}
