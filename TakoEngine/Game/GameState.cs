using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using TakoEngine.Entities;
using TakoEngine.Entities.Cameras;
using TakoEngine.Entities.Lights;
using TakoEngine.Entities.Models;
using TakoEngine.Helpers;
using TakoEngine.Inputs;
using TakoEngine.Maps;
using TakoEngine.Outputs;
using TakoEngine.Physics.Collision;
using TakoEngine.Rendering.Processing;
using TakoEngine.Rendering.Textures;

namespace TakoEngine.Game
{
    public class GameState
    {
        public Camera Camera { get; private set; }
        public List<Actor> Actors { get; } = new List<Actor>();
        public List<Brush> Brushes { get; } = new List<Brush>();
        public List<Light> Lights { get; } = new List<Light>();
        public TextureManager TextureManager { get; } = new TextureManager();

        private Resolution _resolution;
        internal InputState _inputState = new InputState();
        protected int _nextAvailableID = 1;

        private QuadTree _actorQuads;
        private QuadTree _brushQuads;
        private QuadTree _lightQuads;

        public GameState(Resolution resolution)
        {
            _resolution = resolution;

            TextureManager.EnableMipMapping = true;
            TextureManager.EnableAnisotropy = true;
        }

        public void LoadMap(Map map)
        {
            Camera = map.Camera.ToCamera(_resolution);

            LoadLightsFromMap(map);
            LoadBrushesFromMap(map);
            LoadActorsFromMap(map);
        }

        private void LoadLightsFromMap(Map map)
        {
            _lightQuads = new QuadTree(0, map.Boundaries);
            _lightQuads.InsertRange(map.Lights.Select(l => new BoundingCircle(l)));

            foreach (var mapLight in map.Lights)
            {
                AddEntity(mapLight);
            }
        }

        private void LoadBrushesFromMap(Map map)
        {
            _brushQuads = new QuadTree(0, map.Boundaries);

            foreach (var mapBrush in map.Brushes)
            {
                var brush = mapBrush.ToBrush();

                if (brush.HasCollision)
                {
                    _brushQuads.Insert(brush.Bounds);
                }
                brush.AddPointLights(_lightQuads.Retrieve(brush.Bounds).Where(c => c.AttachedEntity is PointLight).Select(c => (PointLight)c.AttachedEntity));
                brush.Mesh.TextureMapping = mapBrush.TexturesPaths.ToTextureMapping(TextureManager);

                AddEntity(brush);
            }
        }

        private void LoadActorsFromMap(Map map)
        {
            _actorQuads = new QuadTree(0, map.Boundaries);

            foreach (var mapActor in map.Actors)
            {
                var actor = mapActor.ToActor(TextureManager);

                switch (actor.Model)
                {
                    case SimpleModel s:
                        for (var i = 0; i < s.Meshes.Count; i++)
                        {
                            if (i < mapActor.TexturesPaths.Count)
                            {
                                s.Meshes[i].TextureMapping = mapActor.TexturesPaths[i].ToTextureMapping(TextureManager);
                            }
                        }
                        break;

                    case AnimatedModel a:
                        for (var i = 0; i < a.Meshes.Count; i++)
                        {
                            if (i < mapActor.TexturesPaths.Count)
                            {
                                a.Meshes[i].TextureMapping = mapActor.TexturesPaths[i].ToTextureMapping(TextureManager);
                            }
                        }
                        break;
                }

                if (map.Camera.AttachedActorName == actor.Name)
                {
                    Camera.AttachToEntity(actor, true, false);
                }

                AddEntity(actor);
            }
        }

        public Actor GetByName(string name) => Actors.First(g => g.Name == name);

        public IEntity GetByID(int id)
        {
            var actor = Actors.FirstOrDefault(g => g.ID == id);
            if (actor != null)
            {
                return actor;
            }
            else
            {
                var brush = Brushes.FirstOrDefault(b => b.ID == id);
                if (brush != null)
                {
                    return brush;
                }
                else
                {
                    var light = Lights.FirstOrDefault(l => l.ID == id);
                    if (light != null)
                    {
                        return light;
                    }
                    else
                    {
                        throw new KeyNotFoundException("Could not find any GameEntity with ID " + id);
                    }
                }
            }
        }

        public virtual void AddEntity(IEntity entity)
        {
            // Assign a unique ID
            //if (entity.ID == 0)
            //{
                entity.ID = _nextAvailableID;
                _nextAvailableID++;

                if (_nextAvailableID == SelectionRenderer.RED_ID || _nextAvailableID == SelectionRenderer.GREEN_ID || _nextAvailableID == SelectionRenderer.BLUE_ID
                    || _nextAvailableID == SelectionRenderer.CYAN_ID || _nextAvailableID == SelectionRenderer.MAGENTA_ID || _nextAvailableID == SelectionRenderer.YELLOW_ID)
                {
                    _nextAvailableID++;
                }
            //}

            switch (entity)
            {
                case Actor actor:
                    if (string.IsNullOrEmpty(actor.Name)) throw new ArgumentException("Actor must have a name defined");
                    if (Actors.Any(g => g.Name == actor.Name)) throw new ArgumentException("Actor must have a unique name");
                    Actors.Add(actor);
                    break;
                case Brush brush:
                    Brushes.Add(brush);
                    break;
                case Light light:
                    Lights.Add(light);
                    break;
            }
        }

        public void Initialize()
        {
            foreach (var actor in Actors)
            {
                actor.OnInitialization();
            }
        }

        public void HandleInput()
        {
            Camera.OnHandleInput(_inputState);

            foreach (var actor in Actors)
            {
                actor.OnHandleInput(_inputState, Camera);
            }
        }

        public void UpdateFrame()
        {
            // Update the gameobject colliders every frame, since they could have moved
            _actorQuads.Clear();
            _actorQuads.InsertRange(Actors.Select(g => g.Bounds).Where(c => c != null));

            // For each object that has a non-zero transform, we need to determine the set of colliders to compare it against for hit detection
            foreach (var actor in Actors)
            {
                actor.ClearLights();
                actor.AddPointLights(_lightQuads.Retrieve(actor.Bounds)
                    .Where(c => c.AttachedEntity is PointLight)
                    .Select(c => (PointLight)c.AttachedEntity));

                var filteredColliders = _brushQuads.Retrieve(actor.Bounds)
                    .Concat(_actorQuads
                        .Retrieve(actor.Bounds)
                            .Where(c => ((Actor)c.AttachedEntity).Name != actor.Name));

                actor.OnUpdateFrame(filteredColliders);
            }

            Camera.OnUpdateFrame();

            PollForInput();
        }

        private void PollForInput() => _inputState.UpdateState(Keyboard.GetState(), Mouse.GetState());

        public void SaveToFile(string path) => throw new NotImplementedException();

        public static GameState LoadFromFile(string path) => throw new NotImplementedException();
    }
}
