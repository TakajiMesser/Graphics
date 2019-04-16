using OpenTK.Graphics.OpenGL;
using SpiceEngine.Entities;
using SpiceEngine.Entities.Actors;
using SpiceEngine.Entities.Cameras;
using SpiceEngine.Helpers;
using SpiceEngine.Inputs;
using SpiceEngine.Maps;
using SpiceEngine.Outputs;
using SpiceEngine.Physics;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Scripting;
using SpiceEngine.Scripting.Scripts;
using SpiceEngine.Sounds;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace SpiceEngine.Game
{
    public class GameManager
    {
        public Camera Camera { get; set; }

        public EntityManager EntityManager { get; } = new EntityManager();
        public InputManager InputManager { get; private set; }
        public PhysicsManager PhysicsManager { get; private set; }
        public BehaviorManager BehaviorManager { get; private set; }
        public ScriptManager ScriptManager { get; private set; }
        public SoundManager SoundManager { get; private set; }

        public bool IsLoaded { get; private set; }

        private Resolution _resolution;

        public GameManager(Resolution resolution)
        {
            _resolution = resolution;

            InputManager = new InputManager();
        }

        public GameManager(Resolution resolution, IMouseDelta mouseDelta)
        {
            _resolution = resolution;

            InputManager = new InputManager(mouseDelta);
        }

        public EntityMapping LoadFromMap(Map map)
        {
            IsLoaded = false;

            switch (map)
            {
                case Map2D map2D:
                    PhysicsManager = new PhysicsManager(EntityManager, map2D.Boundaries);
                    break;
                case Map3D map3D:
                    PhysicsManager = new PhysicsManager(EntityManager, map3D.Boundaries);
                    break;
            }

            Camera = map.Camera.ToCamera(_resolution);

            ScriptManager = new ScriptManager();
            BehaviorManager = new BehaviorManager(EntityManager, PhysicsManager);
            BehaviorManager.SetCamera(Camera);
            BehaviorManager.SetInputProvider(InputManager);

            EntityManager.ClearEntities();

            var lightIDs = LoadLights(map.Lights);
            var brushIDs = LoadBrushes(map.Brushes);
            var volumeIDs = LoadVolumes(map.Volumes);
            var actorIDs = LoadActors(map.Actors);

            var entityMapping = new EntityMapping(actorIDs, brushIDs, volumeIDs, lightIDs);

            var actor = EntityManager.GetActor(map.Camera.AttachedActorName);
            Camera.AttachToEntity(actor, true, false);
            BehaviorManager.Load();

            IsLoaded = true;

            return entityMapping;
        }

        public int AddLight(MapLight mapLight)
        {
            var light = mapLight.ToEntity();
            return EntityManager.AddEntity(light);
        }

        public int AddBrush(MapBrush mapBrush)
        {
            var brush = mapBrush.ToEntity();
            int entityID = EntityManager.AddEntity(brush);

            if (mapBrush.IsPhysical)
            {
                var shape = mapBrush.ToShape();
                PhysicsManager.AddBrush(brush, shape, mapBrush.IsPhysical);
            }

            return entityID;
        }

        public int AddActor(MapActor mapActor)
        {
            var actor = mapActor.ToEntity(/*_gameManager.TextureManager*/);
            int entityID = EntityManager.AddEntity(actor);

            var meshes = mapActor.ToMeshes();

            var shape = mapActor.ToShape();
            PhysicsManager.AddActor(actor, shape, mapActor.IsPhysical);

            /*actor.HasCollision = mapActor.HasCollision;
            actor.Bounds = actor.Name == "Player"
                ? (Bounds)new BoundingCircle(actor, meshes.SelectMany(m => m.Vertices.Select(v => v.Position)))
                : new BoundingBox(actor, meshes.SelectMany(m => m.Vertices.Select(v => v.Position)));*/
            
            if (mapActor.Behavior != null)
            {
                var behavior = mapActor.Behavior.ToBehavior(ScriptManager);
                BehaviorManager.AddBehavior(entityID, behavior);
            }

            BehaviorManager.AddProperties(entityID, mapActor.Properties);
            BehaviorManager.AddStimuli(entityID, mapActor.Stimuli);

            return entityID;
        }

        public int AddVolume(MapVolume mapVolume)
        {
            var volume = mapVolume.ToEntity();
            int entityID = EntityManager.AddEntity(volume);

            var shape = mapVolume.ToShape();
            PhysicsManager.AddVolume(volume, shape);

            return entityID;
        }

        private IEnumerable<int> LoadLights(IEnumerable<MapLight> mapLights)
        {
            foreach (var mapLight in mapLights)
            {
                yield return AddLight(mapLight);
            }
        }

        private IEnumerable<int> LoadBrushes(IEnumerable<MapBrush> mapBrushes)
        {
            foreach (var mapBrush in mapBrushes)
            {
                yield return AddBrush(mapBrush);
            }
        }

        private IEnumerable<int> LoadVolumes(IEnumerable<MapVolume> mapVolumes)
        {
            foreach (var mapVolume in mapVolumes)
            {
                yield return AddVolume(mapVolume);
            }
        }

        private IEnumerable<int> LoadActors(IList<MapActor> mapActors)
        {
            foreach (var mapActor in mapActors)
            {
                yield return AddActor(mapActor);
            }
        }

        public void Update()
        {
            Camera.OnHandleInput(InputManager);
            Camera.OnUpdateFrame();

            PhysicsManager.Tick();
            BehaviorManager.Tick();
            InputManager.Tick();

            foreach (var animatedActor in EntityManager.Actors.OfType<AnimatedActor>())
            {
                animatedActor.UpdateAnimation();
            }
        }

        public void SaveToFile(string path) => throw new NotImplementedException();

        public static GameManager LoadFromFile(string path) => throw new NotImplementedException();

        private void TakeScreenshot()
        {
            var bitmap = new Bitmap(_resolution.Width, _resolution.Height);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, _resolution.Width, _resolution.Height), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.ReadPixels(0, 0, _resolution.Width, _resolution.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            GL.Finish();

            bitmap.UnlockBits(data);
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);

            string fileName = FilePathHelper.SCREENSHOT_PATH + "\\"
                + DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") + "_"
                + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + ".png";

            bitmap.Save(fileName, ImageFormat.Png);
            bitmap.Dispose();
        }

        /*private void LoadLightsFromMap(Map map)
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
                    case AnimatedModel3D a:
                        for (var i = 0; i < a.Meshes.Count; i++)
                        {
                            if (i < mapActor.TexturesPaths.Count)
                            {
                                a.Meshes[i].TextureMapping = mapActor.TexturesPaths[i].ToTextureMapping(TextureManager);
                            }
                        }
                        break;

                    case Model3D<Vertex3D> s:
                        for (var i = 0; i < s.Meshes.Count; i++)
                        {
                            if (i < mapActor.TexturesPaths.Count)
                            {
                                s.Meshes[i].TextureMapping = mapActor.TexturesPaths[i].ToTextureMapping(TextureManager);
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
        }*/
    }
}
