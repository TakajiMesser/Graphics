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
using SpiceEngine.Rendering;
using SpiceEngine.Physics;
using SpiceEngine.Rendering.Textures;
using System.IO;

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
            _renderManager.Load(_gameManager.EntityManager, map.SkyboxTextureFilePaths);

            _gameManager.EntityManager.ClearEntities();
            _gameManager.EntityManager.AddEntities(map.Lights);

            _gameManager.Camera = map.Camera.ToCamera(Resolution);

            switch (map)
            {
                case Map2D map2D:
                    _gameManager.PhysicsManager = new PhysicsManager(_gameManager.EntityManager, map2D.Boundaries);
                    break;
                case Map3D map3D:
                    _gameManager.PhysicsManager = new PhysicsManager(_gameManager.EntityManager, map3D.Boundaries);
                    break;
            }

            //_gameManager.PhysicsManager.InsertBrushes(_gameManager.EntityManager.Brushes.Where(b => b.HasCollision).Select(b => b.Bounds));
            //_gameManager.PhysicsManager.InsertVolumes(_gameManager.EntityManager.Volumes.Select(v => v.Bounds));
            //_gameManager.PhysicsManager.InsertLights(map.Lights.Select(l => new BoundingCircle(l)));

            foreach (var mapBrush in map.Brushes)
            {
                var brush = mapBrush.ToEntity();
                brush.TextureMapping = mapBrush.TexturesPaths.ToTextureMapping(_gameManager.TextureManager);

                int entityID = _gameManager.EntityManager.AddEntity(brush);

                var shape = mapBrush.ToShape();
                _gameManager.PhysicsManager.AddBrush(entityID, shape, brush.Position);

                var mesh = mapBrush.ToMesh();
                _renderManager.BatchManager.AddBrush(entityID, mesh);
            }

            foreach (var mapVolume in map.Volumes)
            {
                var volume = mapVolume.ToEntity();
                int entityID = _gameManager.EntityManager.AddEntity(volume);

                var shape = mapVolume.ToShape();
                _gameManager.PhysicsManager.AddVolume(entityID, shape, volume.Position);
            }

            foreach (var mapActor in map.Actors)
            {
                var actor = mapActor.ToEntity(/*_gameManager.TextureManager*/);

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

                var shape = mapActor.ToShape();
                _gameManager.PhysicsManager.AddActor(entityID, shape, actor.Position);

                /*actor.HasCollision = mapActor.HasCollision;
                actor.Bounds = actor.Name == "Player"
                    ? (Bounds)new BoundingCircle(actor, meshes.SelectMany(m => m.Vertices.Select(v => v.Position)))
                    : new BoundingBox(actor, meshes.SelectMany(m => m.Vertices.Select(v => v.Position)));*/

                if (actor is AnimatedActor)
                {
                    using (var importer = new Assimp.AssimpContext())
                    {
                        var scene = importer.ImportFile(mapActor.ModelFilePath);

                        for (var i = 0; i < scene.Meshes.Count; i++)
                        {
                            var textureMapping = i < mapActor.TexturesPaths.Count
                                ? mapActor.TexturesPaths[i].ToTextureMapping(_gameManager.TextureManager)
                                : new TexturePaths(scene.Materials[scene.Meshes[i].MaterialIndex], Path.GetDirectoryName(mapActor.ModelFilePath)).ToTextureMapping(_gameManager.TextureManager);

                            actor.AddTextureMapping(i, textureMapping);
                        }
                    }
                }
                else
                {
                    for (var i = 0; i < mapActor.TexturesPaths.Count; i++)
                    {
                        actor.AddTextureMapping(i, mapActor.TexturesPaths[i].ToTextureMapping(_gameManager.TextureManager));
                    }
                }

                if (map.Camera.AttachedActorName == actor.Name)
                {
                    _gameManager.Camera.AttachToEntity(actor, true, false);
                }
            }

            //_gameManager.LoadFromMap(map);
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
