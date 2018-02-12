using Graphics.GameObjects;
using Graphics.Lighting;
using Graphics.Maps;
using Graphics.Rendering.Matrices;
using Graphics.Utilities;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsTest.Helpers.Builders
{
    public static class MapBuilder
    {
        public static void CreateTestMap()
        {
            var map = new Map()
            {
                Camera = CreateCameraObject()
            };

            map.GameObjects.Add(CreatePlayerObject());
            map.GameObjects.Add(CreateEnemyObject());

            var floor = MapBrush.Rectangle(new Vector3(0.0f, 0.0f, -1.5f), 50.0f, 50.0f);
            //floor = MapBrush.RectangularPrism(new Vector3(0.0f, 0.0f, -2.5f), 50.0f, 50.0f, 1.0f);
            //floor.TextureFilePath = FilePathHelper.GRASS_TEXTURE_PATH;
            //floor.NormalMapFilePath = FilePathHelper.GRASS_N_TEXTURE_PATH;
            map.Brushes.Add(floor);

            var wall = MapBrush.RectangularPrism(new Vector3(10.0f, 0.0f, 0.0f), 5.0f, 10.0f, 5.0f);
            wall.HasCollision = true;
            wall.DiffuseMapFilePath = FilePathHelper.BRICK_01_D_TEXTURE_PATH;
            wall.NormalMapFilePath = FilePathHelper.BRICK_01_N_NORMAL_PATH;
            map.Brushes.Add(wall);

            var wall2 = MapBrush.RectangularPrism(new Vector3(-10.0f, 0.0f, 0.0f), 5.0f, 10.0f, 5.0f);
            wall2.HasCollision = true;
            wall2.DiffuseMapFilePath = FilePathHelper.BRICK_01_D_TEXTURE_PATH;
            wall2.NormalMapFilePath = FilePathHelper.BRICK_01_N_NORMAL_PATH;
            map.Brushes.Add(wall2);

            map.Lights.Add(new PointLight()
            {
                Position = new Vector3(0.0f, 0.0f, 5.0f),
                Radius = 20.0f,
                Color = new Vector3(1.0f, 1.0f, 1.0f),
                Intensity = 0.5f
            });
            map.Lights.Add(new PointLight()
            {
                Position = new Vector3(0.0f, 20.0f, 3.0f),
                Radius = 30.0f,
                Color = new Vector3(1.0f, 1.0f, 1.0f),
                Intensity = 0.25f
            });
            map.Lights.Add(new SpotLight()
            {
                Position = new Vector3(-17.0f, -2.0f, 3.0f),
                Radius = 10.0f,
                Color = new Vector3(1.0f, 1.0f, 1.0f),
                Intensity = 0.5f,
                Rotation = Quaternion.FromAxisAngle(Vector3.UnitZ, UnitConversions.ToRadians(-45.0f)) * Quaternion.FromAxisAngle(Vector3.UnitY, UnitConversions.ToRadians(-45.0f)),
                Height = 20.0f
            });

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

        private static MapCamera CreateCameraObject()
        {
            /*return new MapCamera()
            {
                Name = "MainCamera",
                AttachedGameObjectName = "Player",
                Position = new Vector3(0.0f, 0.0f, -10.0f),
                Type = ProjectionTypes.Orthographic,
                StartingWidth = 20.0f,
            };*/
            return new MapCamera()
            {
                Name = "MainCamera",
                AttachedGameObjectName = "Player",
                Position = new Vector3(0.0f, 0.0f, 20.0f),
                Type = ProjectionTypes.Perspective,
                FieldOfViewY = UnitConversions.ToRadians(45.0f)
            };
        }

        private static MapGameObject CreatePlayerObject()
        {
            return new MapGameObject()
            {
                Name = "Player",
                Position = new Vector3(0.0f, 0.0f, -1.0f),
                //Position = new Vector3(0.0f, 20.0f, 0.0f),
                Scale = Vector3.One,
                Rotation = Quaternion.Identity,
                ModelFilePath = FilePathHelper.PLAYER_MESH_PATH,
                DiffuseMapFilePath = FilePathHelper.BRICK_01_D_TEXTURE_PATH,
                NormalMapFilePath = FilePathHelper.BRICK_01_N_NORMAL_PATH,
                SpecularMapFilePath = FilePathHelper.BRICK_01_S_TEXTURE_PATH,
                //ParallaxMapFilePath = FilePathHelper.BRICK_01_H_TEXTURE_PATH,
                BehaviorFilePath = FilePathHelper.PLAYER_INPUT_BEHAVIOR_PATH,
                Properties = new List<GameProperty>
                {
                    new GameProperty("WALK_SPEED", typeof(float), 0.1f, true),
                    new GameProperty("RUN_SPEED", typeof(float), 0.15f, true),
                    new GameProperty("CREEP_SPEED", typeof(float), 0.04f, true),
                    new GameProperty("EVADE_SPEED", typeof(float), 0.175f, true),
                    new GameProperty("COVER_SPEED", typeof(float), 0.1f, true),
                    new GameProperty("ENTER_COVER_SPEED", typeof(float), 0.12f, true),
                    new GameProperty("COVER_DISTANCE", typeof(float), 5.0f, true),
                    new GameProperty("EVADE_TICK_COUNT", typeof(int), 20, true)
                }
            };
        }

        private static MapGameObject CreateEnemyObject()
        {
            return new MapGameObject()
            {
                Name = "Enemy",
                Position = new Vector3(5.0f, 5.0f, -1.0f),
                Scale = Vector3.One,
                Rotation = Quaternion.Identity,
                ModelFilePath = FilePathHelper.ENEMY_MESH_PATH,
                //TextureFilePath = FilePathHelper.BRICK_02_D_TEXTURE_PATH,
                //NormalMapFilePath = FilePathHelper.BRICK_02_N_NORMAL_PATH,
                BehaviorFilePath = FilePathHelper.ENEMY_PATROL_BEHAVIOR_PATH,
                Properties = new List<GameProperty>
                {
                    new GameProperty("WALK_SPEED", typeof(float), 0.1f, true),
                    new GameProperty("VIEW_ANGLE", typeof(float), 1.0472f, true),
                    new GameProperty("VIEW_DISTANCE", typeof(float), 5.0f, true)
                }
            };
        }
    }
}
