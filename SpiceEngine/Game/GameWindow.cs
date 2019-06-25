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
    public class GameWindow : OpenTK.GameWindow, IMouseTracker
    {
        private GameManager _gameManager;
        private RenderManager _renderManager;

        private MouseState? _mouseState = null;

        private Timer _fpsTimer = new Timer(1000);
        private List<double> _frequencies = new List<double>();

        private Map _map;
        private object _loadLock = new object();

        public GameWindow(Map map) : base(1280, 720, GraphicsMode.Default, "My First OpenGL Game", GameWindowFlags.Default, DisplayDevice.Default, 3, 0, GraphicsContextFlags.ForwardCompatible)
        {
            _map = map;

            Resolution = new Resolution(Width, Height);
            WindowSize = new Resolution(Width, Height);

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

        public Resolution Resolution { get; private set; }
        public Resolution WindowSize { get; private set; }

        public Vector2? MouseCoordinates => _mouseState.HasValue
            ? new Vector2(_mouseState.Value.X, _mouseState.Value.Y)
            : (Vector2?)null;

        public bool IsMouseInWindow => _mouseState != null
            ? (_mouseState.Value.X.IsBetween(0, WindowSize.Width) && _mouseState.Value.Y.IsBetween(0, WindowSize.Height))
            : false;

        public bool IsLoaded { get; private set; }

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

            _gameManager = new GameManager(Resolution, this);
            _gameManager.InputManager.EscapePressed += (s, args) => Close();

            _renderManager = new RenderManager(Resolution, WindowSize);
            _fpsTimer.Start();

            lock (_loadLock)
            {
                IsLoaded = true;

                if (_map != null)
                {
                    var entityMapping = _gameManager.LoadFromMap(_map);
                    _renderManager.SetEntityProvider(_gameManager.EntityManager);
                    _renderManager.SetCamera(_gameManager.Camera);
                    _renderManager.LoadFromMap(_map, entityMapping);
                }
            }
        }

        /*public void LoadMap(Map map)
        {
            lock (_loadLock)
            {
                _map = map;

                if (IsLoaded)
                {
                    var entityMapping = _gameManager.LoadFromMap(_map);
                    _renderManager.LoadFromMap(_map, _gameManager.EntityManager, entityMapping);
                }
            }
        }*/

        //protected override void OnMouseEnter(EventArgs e) => CursorVisible = false;

        //protected override void OnMouseLeave(EventArgs e) => CursorVisible = true;

        // Handle game logic, guaranteed to run at a fixed rate, regardless of FPS
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            //OpenTK.Input.
            //_mouseDevice = InputDriver.Mouse;
            //_mouseDevice = Mouse;
            _mouseState = Mouse.GetCursorState();
            _gameManager.Update();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            _frequencies.Add(RenderFrequency);
            _renderManager.RenderFullFrame();

            GL.UseProgram(0);
            SwapBuffers();
        }
    }
}
