using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using SpiceEngine.Inputs;
using SpiceEngine.Maps;
using SpiceEngine.Outputs;
using SpiceEngine.Rendering;
using SpiceEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Timers;

namespace SpiceEngine.Game
{
    public class GameWindow : OpenTK.GameWindow, IMouseDelta
    {
        private string _mapPath;

        private GameManager _gameManager;
        private RenderManager _renderManager;

        private MouseDevice _mouseDevice;

        private Timer _fpsTimer = new Timer(1000);
        private List<double> _frequencies = new List<double>();

        public Resolution Resolution { get; private set; }
        public Resolution WindowSize { get; private set; }

        public GameWindow(string mapPath) : base(1280, 720, GraphicsMode.Default, "My First OpenGL Game", GameWindowFlags.Default, DisplayDevice.Default, 3, 0, GraphicsContextFlags.ForwardCompatible)
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

        public Vector2? MouseCoordinates => _mouseDevice != null
            ? new Vector2(_mouseDevice.X, _mouseDevice.Y)
            : (Vector2?)null;

        public bool IsMouseInWindow => _mouseDevice != null
            ? (_mouseDevice.X.IsBetween(0, WindowSize.Width) && _mouseDevice.Y.IsBetween(0, WindowSize.Height))
            : false;

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

            _gameManager = new GameManager(Resolution, this);
            _gameManager.InputManager.EscapePressed += (s, args) => Close();
            var entityMapping = _gameManager.LoadFromMap(map);

            _renderManager = new RenderManager(Resolution, WindowSize);
            _renderManager.LoadFromMap(map, _gameManager.EntityManager, entityMapping);

            _fpsTimer.Start();
        }

        //protected override void OnMouseEnter(EventArgs e) => CursorVisible = false;

        //protected override void OnMouseLeave(EventArgs e) => CursorVisible = true;

        // Handle game logic, guaranteed to run at a fixed rate, regardless of FPS
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            _mouseDevice = Mouse;
            _gameManager.Update();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            _frequencies.Add(RenderFrequency);
            _renderManager.RenderFullFrame(_gameManager.EntityManager, _gameManager.Camera, _gameManager.TextureManager);

            GL.UseProgram(0);
            SwapBuffers();
        }
    }
}
