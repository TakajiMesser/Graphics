using OpenTK.Graphics.OpenGL;
using SpiceEngine.Maps;
using SpiceEngine.Physics;
using SpiceEngine.Scripting;
using SpiceEngine.UserInterfaces;
using SpiceEngineCore.Components.Animations;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Helpers;
using SpiceEngineCore.Inputs;
using SpiceEngineCore.Maps;
using SpiceEngineCore.Outputs;
using SpiceEngineCore.Sounds;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace SpiceEngine.Game
{
    public class GameManager
    {
        private Resolution _resolution;

        public GameManager(Resolution resolution)
        {
            _resolution = resolution;
            InputManager = new InputManager();
        }

        public GameManager(Resolution resolution, IMouseTracker mouseTracker)
        {
            _resolution = resolution;
            InputManager = new InputManager(mouseTracker);
        }

        public ICamera Camera { get; set; }

        public EntityManager EntityManager { get; } = new EntityManager();
        public InputManager InputManager { get; private set; }
        public PhysicsManager PhysicsManager { get; private set; }
        public BehaviorManager BehaviorManager { get; private set; }
        public AnimationManager AnimationManager { get; private set; }
        public UIManager UIManager { get; private set; }
        public SoundManager SoundManager { get; private set; }

        public bool IsLoaded { get; private set; }

        public void LoadFromMap(IMap map)
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

            //Camera = map.GetCameraAt(0).ToEntity() as ICamera;

            BehaviorManager = new BehaviorManager(EntityManager, PhysicsManager);
            //BehaviorManager.SetCamera(Camera);
            BehaviorManager.SetInputProvider(InputManager);

            AnimationManager = new AnimationManager(EntityManager);
            UIManager = new UIManager(EntityManager, _resolution);
            BehaviorManager.SetUIProvider(UIManager);

            EntityManager.ClearEntities();

            IsLoaded = true;
        }

        /*public int AddLight(MapLight mapLight)
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
                PhysicsManager.AddBrush((Entities.Brushes.Brush)brush, shape, mapBrush.IsPhysical);
            }

            return entityID;
        }

        public int AddActor(MapActor mapActor)
        {
            var actor = mapActor.ToEntity(/*_gameManager.TextureManager*);
            int entityID = EntityManager.AddEntity(actor);

            //var meshes = mapActor.ToMeshes();

            var shape = mapActor.ToShape();
            PhysicsManager.AddActor((Actor)actor, shape, mapActor.IsPhysical);

            /*actor.HasCollision = mapActor.HasCollision;
            actor.Bounds = actor.Name == "Player"
                ? (Bounds)new BoundingCircle(actor, meshes.SelectMany(m => m.Vertices.Select(v => v.Position)))
                : new BoundingBox(actor, meshes.SelectMany(m => m.Vertices.Select(v => v.Position)));*
            
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
            PhysicsManager.AddVolume((Volume)volume, shape);

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
        }*/

        public void Update()
        {
            PhysicsManager.Tick();
            BehaviorManager.Tick();
            AnimationManager.Tick();
            UIManager.Tick();
            InputManager.Tick();
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
    }
}
