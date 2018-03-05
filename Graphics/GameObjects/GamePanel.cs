using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.IO;
using Graphics;
using Graphics.Meshes;
using Graphics.GameObjects;
using System.Runtime.InteropServices;
using OpenTK.Input;
using Graphics.Physics.Collision;
using Graphics.Helpers;
using Graphics.Maps;
using Graphics.Rendering.Shaders;
using Graphics.Scripting.BehaviorTrees;
using Graphics.Rendering.PostProcessing;
using Graphics.Outputs;
using System.Drawing;
using System.Drawing.Imaging;
using System.Timers;
using System.Windows.Threading;
using System.Windows.Forms;
using Graphics.Inputs;
using Graphics.Rendering.Processing;
using Graphics.Rendering.Textures;
using Graphics.Lighting;

namespace Graphics.GameObjects
{
    public class GamePanel : OpenTK.GLControl
    {
        private string _mapPath;
        private GameState _gameState;

        private KeyboardState _keyState;
        private KeyboardState _previousKeyState;
        private MouseState _mouseState;
        private MouseState _previousMouseState;

        internal InputState _inputState = new InputState();

        private DispatcherTimer _timer = new DispatcherTimer(DispatcherPriority.Render);
        private System.Timers.Timer _fpsTimer = new System.Timers.Timer(1000);
        private List<double> _frequencies = new List<double>();

        public double Frequency { get; private set; }
        public Resolution Resolution { get; private set; }
        public EventHandler Resized;

        public GamePanel(string mapPath) : base(GraphicsMode.Default, 3, 0, GraphicsContextFlags.ForwardCompatible)
        {
            Resolution = new Resolution(Width, Height);

            _mapPath = mapPath;
            //Console.WriteLine("GL Version: " + GL.GetString(StringName.Version));

            _fpsTimer.Elapsed += (s, e) =>
            {
                if (_frequencies.Count > 0)
                {
                    _gameState?.SetFrequency(_frequencies.Average());
                    _frequencies.Clear();
                }
            };
            _fpsTimer.Start();

            _timer.Interval = TimeSpan.FromMilliseconds(16.67);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            OnUpdateFrame();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            MakeCurrent();
            OnRenderFrame();
        }

        protected override void OnResize(EventArgs e)
        {
            if (Resolution != null)
            {
                Resolution.Width = Width;
                Resolution.Height = Height;
            }

            Resized?.Invoke(this, new EventArgs());
        }

        protected override void OnLoad(EventArgs e)
        {
            Location = new Point(0, 0);
            //WindowState = WindowState.Maximized;
            //Size = new System.Drawing.Size(1280, 720);

            var loadedMap = Map.Load(_mapPath);

            _gameState = new GameState(loadedMap, new Resolution(800, 600));
            _gameState.Initialize();
        }

        //protected override void OnMouseEnter(EventArgs e) => CursorVisible = false;

        //protected override void OnMouseLeave(EventArgs e) => CursorVisible = true;

        protected void OnUpdateFrame()
        {
            // Handle game logic, guaranteed to run at a fixed rate, regardless of FPS
            HandleInput();
            _gameState.HandleInput();

            _gameState.UpdateFrame();
            PollForInput();
        }

        protected void OnRenderFrame()
        {
            _frequencies.Add(Frequency);
            _gameState.RenderFrame();

            GL.UseProgram(0);
            SwapBuffers();
        }

        private void HandleInput()
        {
            /*if (_keyState.IsKeyDown(Key.Escape))
            {
                Close();
            }

            if (_previousKeyState != null && _previousKeyState.IsKeyUp(Key.F11) && _keyState.IsKeyDown(Key.F11))
            {
                if (WindowState == WindowState.Normal)
                {
                    WindowState = WindowState.Fullscreen;
                }
                else
                {
                    WindowState = WindowState.Normal;
                }
            }*/

            if (_previousKeyState != null && _previousKeyState.IsKeyUp(Key.F11) && _keyState.IsKeyDown(Key.F5))
            {
                TakeScreenshot();
            }
        }

        private void TakeScreenshot()
        {
            var bitmap = new Bitmap(Resolution.Width, Resolution.Height);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, Resolution.Width, Resolution.Height), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.ReadPixels(0, 0, Resolution.Width, Resolution.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            GL.Finish();

            bitmap.UnlockBits(data);
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);

            string fileName = FilePathHelper.SCREENSHOT_PATH + "\\" 
                + DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") + "_" 
                + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + ".png";

            bitmap.Save(fileName, ImageFormat.Png);
            bitmap.Dispose();
        }

        private Point _mouseLocation;

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            _mouseLocation = e.Location;
        }

        private void PollForInput()
        {
            _previousKeyState = _keyState;
            _previousMouseState = _mouseState;
            //_previousMouse = _mouse;

            _keyState = Keyboard.GetState();
            _mouseState = Mouse.GetState();
            //_gameState._inputState.UpdateState(_mouseLocation);
            //_mouse = Mouse;
        }
    }
}
