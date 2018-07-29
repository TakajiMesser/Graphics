using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SpiceEngine.Entities;
using SpiceEngine.Entities.Cameras;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Entities.Models;
using SpiceEngine.Helpers;
using SpiceEngine.Inputs;
using SpiceEngine.Maps;
using SpiceEngine.Outputs;
using SpiceEngine.Physics.Collision;
using SpiceEngine.Rendering.Processing;
using SpiceEngine.Rendering.Textures;

namespace SpiceEngine.Game
{
    public class GameState
    {
        public Camera Camera { get; set; }
        public EntityManager EntityManager { get; private set; } = new EntityManager();
        public TextureManager TextureManager { get; } = new TextureManager();
        public bool IsLoaded { get; private set; }

        private Resolution _resolution;
        internal InputState _inputState = new InputState();

        private QuadTree _actorQuads;
        private QuadTree _brushQuads;
        private QuadTree _volumeQuads;
        private QuadTree _lightQuads;

        public GameState(Resolution resolution)
        {
            _resolution = resolution;

            TextureManager.EnableMipMapping = true;
            TextureManager.EnableAnisotropy = true;
        }

        public void LoadFromEntities(EntityManager entityManager, Map map)
        {
            EntityManager = entityManager;

            _lightQuads = new QuadTree(0, map.Boundaries);
            _lightQuads.InsertRange(EntityManager.Lights.Select(l => new BoundingCircle(l)));

            _brushQuads = new QuadTree(0, map.Boundaries);
            _brushQuads.InsertRange(EntityManager.Brushes.Where(b => b.HasCollision).Select(b => b.Bounds));

            _volumeQuads = new QuadTree(0, map.Boundaries);
            _volumeQuads.InsertRange(EntityManager.Volumes.Select(v => v.Bounds));

            _actorQuads = new QuadTree(0, map.Boundaries);

            for (var i = 0; i < map.Brushes.Count; i++)
            {
                EntityManager.Brushes[i].Mesh.TextureMapping = map.Brushes[i].TexturesPaths.ToTextureMapping(TextureManager);
            }

            foreach (var mapActor in map.Actors)
            {
                var actor = GetActorByName(mapActor.Name);

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
                        using (var importer = new Assimp.AssimpContext())
                        {
                            var scene = importer.ImportFile(mapActor.ModelFilePath);
                            for (var i = 0; i < a.Meshes.Count; i++)
                            {
                                a.Meshes[i].TextureMapping = (i < mapActor.TexturesPaths.Count)
                                    ? mapActor.TexturesPaths[i].ToTextureMapping(TextureManager)
                                    : new TexturePaths(scene.Materials[scene.Meshes[i].MaterialIndex], Path.GetDirectoryName(mapActor.ModelFilePath)).ToTextureMapping(TextureManager);
                            }
                        }
                        break;
                }
            }

            IsLoaded = true;
        }

        public void LoadFromMap(Map map)
        {
            Camera = map.Camera.ToCamera(_resolution);
            EntityManager.ClearEntities();

            LoadLightsFromMap(map);
            LoadBrushesFromMap(map);
            LoadVolumesFromMap(map);
            LoadActorsFromMap(map);
            EntityManager.LoadEntities();

            IsLoaded = true;
        }

        private void LoadLightsFromMap(Map map)
        {
            _lightQuads = new QuadTree(0, map.Boundaries);
            _lightQuads.InsertRange(map.Lights.Select(l => new BoundingCircle(l)));

            EntityManager.AddEntities(map.Lights);
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
                //brush.AddPointLights(_lightQuads.Retrieve(brush.Bounds).Where(c => c.AttachedEntity is PointLight).Select(c => (PointLight)c.AttachedEntity));
                brush.Mesh.TextureMapping = mapBrush.TexturesPaths.ToTextureMapping(TextureManager);

                EntityManager.AddEntity(brush);
            }
        }

        private void LoadVolumesFromMap(Map map)
        {
            _volumeQuads = new QuadTree(0, map.Boundaries);

            EntityManager.AddEntities(map.Volumes.Select(v => v.ToVolume()));
            _volumeQuads.InsertRange(EntityManager.Volumes.Select(v => v.Bounds));
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

                EntityManager.AddEntity(actor);
            }
        }

        public Actor GetActorByName(string name) => EntityManager.Actors.First(g => g.Name == name);

        public void Initialize()
        {
            foreach (var actor in EntityManager.Actors)
            {
                actor.OnInitialization();
            }
        }

        public void HandleInput()
        {
            Camera.OnHandleInput(_inputState);

            foreach (var actor in EntityManager.Actors)
            {
                actor.OnHandleInput(_inputState, Camera);
            }
        }

        public void UpdateFrame()
        {
            // Update the gameobject colliders every frame, since they could have moved
            _actorQuads.Clear();
            _actorQuads.InsertRange(EntityManager.Actors.Select(g => g.Bounds).Where(c => c != null));

            // For each object that has a non-zero transform, we need to determine the set of colliders to compare it against for hit detection
            foreach (var actor in EntityManager.Actors)
            {
                //actor.ClearLights();
                //actor.AddPointLights(_lightQuads.Retrieve(actor.Bounds)
                //    .Where(c => c.AttachedEntity is PointLight)
                //    .Select(c => (PointLight)c.AttachedEntity));

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
