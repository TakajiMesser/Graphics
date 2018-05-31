using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Timers;
using TakoEngine.Helpers;
using TakoEngine.Maps;
using TakoEngine.Outputs;
using TakoEngine.Rendering.Processing;

namespace TakoEngine.Game
{
    public class GameWindow : OpenTK.GameWindow
    {
        public Resolution Resolution { get; private set; }
        public Resolution WindowSize { get; private set; }

        private string _mapPath;

        private GameState _gameState;
        private RenderManager _renderManager;

        private KeyboardState _keyState;
        private KeyboardState _previousKeyState;
        private MouseState _mouseState;
        private MouseState _previousMouseState;
        private MouseDevice _mouse;
        private MouseDevice _previousMouse;

        private Timer _fpsTimer = new Timer(1000);
        private List<double> _frequencies = new List<double>();

        public GameWindow(string mapPath) : base(1280, 720,
            GraphicsMode.Default, "My First OpenGL Game", GameWindowFlags.Default, DisplayDevice.Default, 3, 0, GraphicsContextFlags.ForwardCompatible)
        {
            Resolution = new Resolution(Width, Height);
            WindowSize = new Resolution(Width, Height);

            _mapPath = mapPath;
            Console.WriteLine("GL Version: " + GL.GetString(StringName.Version));

            _fpsTimer.Elapsed += (s, e) =>
            {
                if (_frequencies.Count > 0)
                {
                    _renderManager.Frequency = _frequencies.Average();
                    _frequencies.Clear();
                }
            };
        }

        protected override void OnResize(EventArgs e)
        {
            WindowSize.Width = Width;
            WindowSize.Height = Height;

            if (_renderManager != null && _renderManager.IsLoaded)
            {
                _renderManager.ResizeWindow();
            }
            //Resolution.Width = Width;
            //Resolution.Height = Height;
            //_gameState?.Resize();
        }

        protected override void OnLoad(EventArgs e)
        {
            Location = new Point(0, 0);
            //WindowState = WindowState.Maximized;
            //Size = new System.Drawing.Size(1280, 720);

            var map = Map.Load(_mapPath);

            _gameState = new GameState(Resolution);
            _gameState.LoadMap(map);
            _gameState.Initialize();

            _renderManager = new RenderManager(Resolution, WindowSize);
            _renderManager.Load(map);

            _fpsTimer.Start();
        }

        //protected override void OnMouseEnter(EventArgs e) => CursorVisible = false;

        //protected override void OnMouseLeave(EventArgs e) => CursorVisible = true;

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            // Handle game logic, guaranteed to run at a fixed rate, regardless of FPS
            HandleInput();
            _gameState.HandleInput();

            _gameState.UpdateFrame();
            PollForInput();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            _frequencies.Add(RenderFrequency);
            _renderManager.RenderFullFrame(_gameState.Camera, _gameState.EntityManager, _gameState.TextureManager);

            GL.UseProgram(0);
            SwapBuffers();
        }

        private void HandleInput()
        {
            if (_keyState.IsKeyDown(Key.Escape))
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
            }

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

        private void PollForInput()
        {
            _previousKeyState = _keyState;
            _previousMouseState = _mouseState;
            _previousMouse = _mouse;

            _keyState = Keyboard.GetState();
            _mouseState = Mouse.GetState();
            _mouse = Mouse;

            _gameState?._inputState.UpdateState(this);
        }
    }
}
