using Jidai.GameObjects;
using OpenTK;
using OpenTK.Graphics;
using SpiceEngine.Maps;
using SpiceEngine.Rendering.Textures;
using System.Collections.Generic;
using static SpiceEngine.Maps.MapLight;

namespace Jidai.Helpers.Builders
{
    public static class MapBuilder
    {
        public static void GenerateTestMap(string filePath)
        {
            var map = new Map3D()
            {
                Camera = new Jidai.GameObjects.Camera()
            };

            map.Actors.AddRange(GenerateActors());
            map.Brushes.AddRange(GenerateBrushes());
            map.Lights.AddRange(GenerateLights());
            map.Volumes.AddRange(GenerateVolumes());

            map.SkyboxTextureFilePaths = new List<string>
            {
                FilePathHelper.SPACE_01_TEXTURE_PATH,
                FilePathHelper.SPACE_02_TEXTURE_PATH,
                FilePathHelper.SPACE_03_TEXTURE_PATH,
                FilePathHelper.SPACE_04_TEXTURE_PATH,
                FilePathHelper.SPACE_05_TEXTURE_PATH,
                FilePathHelper.SPACE_06_TEXTURE_PATH,
            };

            map.Save(filePath);
        }

        private static IEnumerable<MapActor> GenerateActors()
        {
            yield return new Player();
            yield return new Enemy();
        }

        private static IEnumerable<MapBrush> GenerateBrushes()
        {
            var floor = MapBrush.Rectangle(new Vector3(0.0f, 0.0f, -1.5f), 50.0f, 50.0f);
            floor.IsPhysical = true;
            //floor = MapBrush.Box(new Vector3(0.0f, 0.0f, -2.5f), 50.0f, 50.0f, 1.0f);
            //floor.TextureFilePath = FilePathHelper.GRASS_TEXTURE_PATH;
            //floor.NormalMapFilePath = FilePathHelper.GRASS_N_TEXTURE_PATH;
            yield return floor;

            var wall = MapBrush.Box(new Vector3(10.0f, 0.0f, 0.0f), 5.0f, 10.0f, 5.0f);
            wall.IsPhysical = true;
            wall.TexturesPaths = new TexturePaths()
            {
                DiffuseMapFilePath = FilePathHelper.BRICK_01_D_TEXTURE_PATH,
                NormalMapFilePath = FilePathHelper.BRICK_01_N_NORMAL_PATH
            };
            yield return wall;

            var wall2 = MapBrush.Box(new Vector3(-10.0f, 0.0f, 0.0f), 5.0f, 10.0f, 5.0f);
            wall2.IsPhysical = true;
            wall2.TexturesPaths = new TexturePaths()
            {
                DiffuseMapFilePath = FilePathHelper.BRICK_01_D_TEXTURE_PATH,
                NormalMapFilePath = FilePathHelper.BRICK_01_N_NORMAL_PATH
            };
            yield return wall2;
        }

        private static IEnumerable<MapLight> GenerateLights()
        {
            yield return new MapLight()
            {
                LightType = LightTypes.Point,
                Position = new Vector3(0.0f, 0.0f, 5.0f),
                Radius = 20.0f,
                Color = new Color4(1.0f, 1.0f, 1.0f, 1.0f),
                Intensity = 0.5f
            };

            yield return new MapLight()
            {
                LightType = LightTypes.Point,
                Position = new Vector3(0.0f, 20.0f, 3.0f),
                Radius = 30.0f,
                Color = new Color4(1.0f, 1.0f, 1.0f, 1.0f),
                Intensity = 0.25f
            };

            yield return new MapLight()
            {
                LightType = LightTypes.Spot,
                Position = new Vector3(-17.0f, -2.0f, 3.0f),
                Radius = 10.0f,
                Color = new Color4(1.0f, 1.0f, 1.0f, 1.0f),
                Intensity = 0.5f,
                Rotation = new Vector3(0.0f, -45.0f, -45.0f),//Quaternion.FromAxisAngle(Vector3.UnitZ, UnitConversions.ToRadians(-45.0f)) * Quaternion.FromAxisAngle(Vector3.UnitY, UnitConversions.ToRadians(-45.0f)),
                Height = 20.0f
            };
        }

        private static IEnumerable<MapVolume> GenerateVolumes()
        {
            var physicsVolume = MapVolume.Box(Vector3.Zero, 20.0f, 20.0f, 20.0f);
            physicsVolume.VolumeType = MapVolume.VolumeTypes.Physics;
            physicsVolume.Gravity = -0.3f * Vector3.UnitZ;
            yield return physicsVolume;
        }
    }
}
