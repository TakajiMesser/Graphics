using OpenTK.Graphics.OpenGL;
using SpiceEngine.Entities;
using SpiceEngine.Entities.Cameras;
using SpiceEngine.Entities.Models;
using SpiceEngine.Helpers;
using SpiceEngine.Inputs;
using SpiceEngine.Maps;
using SpiceEngine.Outputs;
using SpiceEngine.Physics;
using SpiceEngine.Physics.Collision;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;
using SpiceEngine.Scripting;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace SpiceEngine.Game
{
    public class GameManager
    {
        public Camera Camera { get; set; }

        public EntityManager EntityManager { get; private set; } = new EntityManager();
        public TextureManager TextureManager { get; } = new TextureManager();
        public InputManager InputManager { get; private set; }
        public PhysicsManager PhysicsManager { get; set; }
        public ScriptManager ScriptManager { get; set; }

        public bool IsLoaded { get; private set; }

        private Resolution _resolution;

        public GameManager(Resolution resolution, IMouseDelta mouseDelta)
        {
            _resolution = resolution;

            InputManager = new InputManager(mouseDelta);

            TextureManager.EnableMipMapping = true;
            TextureManager.EnableAnisotropy = true;

            ScriptManager = new ScriptManager(EntityManager);
        }

        public void LoadFromEntities(EntityManager entityManager, Map map)
        {
            EntityManager = entityManager;

            /*foreach (var mapActor in map.Actors)
            {
                var actor = EntityManager.GetActorByName(mapActor.Name);

                switch (actor.Model)
                {
                    case Model3D<Vertex3D> s:
                        for (var i = 0; i < s.Meshes.Count; i++)
                        {
                            if (i < mapActor.TexturesPaths.Count)
                            {
                                s.Meshes[i].TextureMapping = mapActor.TexturesPaths[i].ToTextureMapping(TextureManager);
                            }
                        }
                        break;

                    case AnimatedModel3D a:
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
            }*/

            IsLoaded = true;
        }

        public void LoadFromMap(Map map)
        {
            Camera = map.Camera.ToCamera(_resolution);

            //EntityManager.ClearEntities();
            //EntityManager.AddEntities(map.Lights);
            //EntityManager.AddEntities(map.Brushes.Select(b => b.ToBrush()));
            //EntityManager.AddEntities(map.Volumes.Select(v => v.ToVolume()));
            //EntityManager.AddEntities(map.Actors.Select(a => a.ToActor()));
            //EntityManager.LoadEntities();

            /*foreach (var mapBrush in map.Brushes)
            {
                var brush = mapBrush.ToBrush();
                brush.TextureMapping = mapBrush.TexturesPaths.ToTextureMapping(TextureManager);

                int entityID = EntityManager.AddEntity(brush);
            }*/

            switch (map)
            {
                case Map2D map2D:
                    PhysicsManager = new PhysicsManager(EntityManager, map2D.Boundaries);
                    break;
                case Map3D map3D:
                    PhysicsManager = new PhysicsManager(EntityManager, map3D.Boundaries);
                    break;
            }

            //PhysicsManager.InsertBrushes(EntityManager.Brushes.Where(b => b.HasCollision).Select(b => b.Bounds));
            //PhysicsManager.InsertVolumes(EntityManager.Volumes.Select(v => v.Bounds));
            //PhysicsManager.InsertLights(map.Lights.Select(l => new BoundingCircle(l)));

            foreach (var mapActor in map.Actors)
            {
                var actor = EntityManager.GetActorByName(mapActor.Name);

                if (actor is AnimatedActor)
                {
                    using (var importer = new Assimp.AssimpContext())
                    {
                        var scene = importer.ImportFile(mapActor.ModelFilePath);

                        for (var i = 0; i < scene.Meshes.Count; i++)
                        {
                            var textureMapping = i < mapActor.TexturesPaths.Count
                                ? mapActor.TexturesPaths[i].ToTextureMapping(TextureManager)
                                : new TexturePaths(scene.Materials[scene.Meshes[i].MaterialIndex], Path.GetDirectoryName(mapActor.ModelFilePath)).ToTextureMapping(TextureManager);

                            actor.AddTextureMapping(i, textureMapping);
                        }
                    }
                }
                else
                {
                    for (var i = 0; i < mapActor.TexturesPaths.Count; i++)
                    {
                        actor.AddTextureMapping(i, mapActor.TexturesPaths[i].ToTextureMapping(TextureManager));
                    }
                }

                if (map.Camera.AttachedActorName == actor.Name)
                {
                    Camera.AttachToEntity(actor, true, false);
                }
            }

            IsLoaded = true;
        }

        public void Update()
        {
            Camera.OnHandleInput(InputManager);

            ScriptManager.HandleInput(InputManager, Camera);
            PhysicsManager.Update();

            ScriptManager.UpdatePhysics(PhysicsManager.ActorPhysics);
            ScriptManager.Update();

            PhysicsManager.HandleActorCollisions(ScriptManager.ActorTranslations);

            Camera.OnUpdateFrame();

            InputManager.Update();
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
