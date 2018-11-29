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
using SpiceEngine.Helpers;
using SpiceEngine.Maps;
using SpiceEngine.Outputs;
using SpiceEngine.Rendering.Processing;
using SpiceEngine.Utilities;
using SpiceEngine.Inputs;
using SpiceEngine.Physics.Collision;
using SpiceEngine.Entities;

namespace SpiceEngine.Game
{
    public class GameWindow : OpenTK.GameWindow, IMouseDelta
    {
        public Resolution Resolution { get; private set; }
        public Resolution WindowSize { get; private set; }

        private string _mapPath;
        private GameManager _gameManager;
        private RenderManager _renderManager;

        private MouseDevice _mouseDevice;

        private Timer _fpsTimer = new Timer(1000);
        private List<double> _frequencies = new List<double>();

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

            /*_gameState = new GameState(Resolution);
            _gameState.LoadFromMap(map);
            _gameState.Initialize();*/

            _gameManager = new GameManager(Resolution, this/*, map*/);
            _renderManager = new RenderManager(Resolution, WindowSize);
            _renderManager.Load(map.SkyboxTextureFilePaths);

            _gameManager.EntityManager.ClearEntities();
            _gameManager.EntityManager.AddEntities(map.Lights);

            foreach (var mapBrush in map.Brushes)
            {
                var brush = mapBrush.ToBrush();
                brush.TextureMapping = mapBrush.TexturesPaths.ToTextureMapping(_gameManager.TextureManager);

                int entityID = _gameManager.EntityManager.AddEntity(brush);

                var mesh = mapBrush.ToMesh();
                _renderManager.BatchManager.AddBrush(entityID, mesh);
            }

            foreach (var mapVolume in map.Volumes)
            {
                var volume = mapVolume.ToVolume();
                int entityID = _gameManager.EntityManager.AddEntity(volume);
            }

            foreach (var mapActor in map.Actors)
            {
                var actor = mapActor.ToActor(_gameManager.TextureManager);

                int entityID = _gameManager.EntityManager.AddEntity(actor);

                var meshes = mapActor.ToMeshes();

                if (actor is AnimatedActor)
                {
                    _renderManager.BatchManager.AddJoint(entityID, meshes);
                }
                else
                {
                    _renderManager.BatchManager.AddActor(entityID, meshes);
                }

                actor.HasCollision = mapActor.HasCollision;
                actor.Bounds = actor.Name == "Player"
                    ? (Bounds)new BoundingCircle(actor, meshes.SelectMany(m => m.Vertices.Select(v => v.Position)))
                    : new BoundingBox(actor, meshes.SelectMany(m => m.Vertices.Select(v => v.Position)));
            }

            _gameManager.LoadFromMap(map);
            _gameManager.Initialize();

            _renderManager.BatchManager.Load();

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
