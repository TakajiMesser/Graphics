using Jidai.GameObjects;
using OpenTK;
using System.Collections.Generic;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Maps;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Utilities;

namespace Jidai.Helpers.Builders
{
    public static class MapBuilder
    {
        public static void CreateTestMap()
        {
            var map = new Map3D()
            {
                Camera = new Jidai.GameObjects.Camera()
            };

            map.Actors.Add(new Player());
            map.Actors.Add(new Enemy());

            var floor = MapBrush.Rectangle(new Vector3(0.0f, 0.0f, -1.5f), 50.0f, 50.0f);
            //floor = MapBrush.RectangularPrism(new Vector3(0.0f, 0.0f, -2.5f), 50.0f, 50.0f, 1.0f);
            //floor.TextureFilePath = FilePathHelper.GRASS_TEXTURE_PATH;
            //floor.NormalMapFilePath = FilePathHelper.GRASS_N_TEXTURE_PATH;
            map.Brushes.Add(floor);

            var wall = MapBrush.RectangularPrism(new Vector3(10.0f, 0.0f, 0.0f), 5.0f, 10.0f, 5.0f);
            wall.HasCollision = true;
            wall.TexturesPaths = new TexturePaths()
            {
                DiffuseMapFilePath = FilePathHelper.BRICK_01_D_TEXTURE_PATH,
                NormalMapFilePath = FilePathHelper.BRICK_01_N_NORMAL_PATH
            };
            map.Brushes.Add(wall);

            var wall2 = MapBrush.RectangularPrism(new Vector3(-10.0f, 0.0f, 0.0f), 5.0f, 10.0f, 5.0f);
            wall2.HasCollision = true;
            wall2.TexturesPaths = new TexturePaths()
            {
                DiffuseMapFilePath = FilePathHelper.BRICK_01_D_TEXTURE_PATH,
                NormalMapFilePath = FilePathHelper.BRICK_01_N_NORMAL_PATH
            };
            map.Brushes.Add(wall2);

            map.Lights.Add(new PointLight()
            {
                Position = new Vector3(0.0f, 0.0f, 5.0f),
                Radius = 20.0f,
                Color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                Intensity = 0.5f
            });
            map.Lights.Add(new PointLight()
            {
                Position = new Vector3(0.0f, 20.0f, 3.0f),
                Radius = 30.0f,
                Color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                Intensity = 0.25f
            });
            map.Lights.Add(new SpotLight()
            {
                Position = new Vector3(-17.0f, -2.0f, 3.0f),
                Radius = 10.0f,
                Color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                Intensity = 0.5f,
                Rotation = Quaternion.FromAxisAngle(Vector3.UnitZ, UnitConversions.ToRadians(-45.0f)) * Quaternion.FromAxisAngle(Vector3.UnitY, UnitConversions.ToRadians(-45.0f)),
                Height = 20.0f
            });

            var physicsVolume = MapVolume.RectangularPrism(Vector3.Zero, 10.0f, 10.0f, 20.0f);
            physicsVolume.VolumeType = MapVolume.VolumeTypes.Physics;
            physicsVolume.Gravity = -0.3f * Vector3.UnitZ;
            map.Volumes.Add(physicsVolume);

            map.SkyboxTextureFilePaths = new List<string>
            {
                FilePathHelper.SPACE_01_TEXTURE_PATH,
                FilePathHelper.SPACE_02_TEXTURE_PATH,
                FilePathHelper.SPACE_03_TEXTURE_PATH,
                FilePathHelper.SPACE_04_TEXTURE_PATH,
                FilePathHelper.SPACE_05_TEXTURE_PATH,
                FilePathHelper.SPACE_06_TEXTURE_PATH,
            };

            map.Save(FilePathHelper.MAP_PATH);
        }
    }
}
