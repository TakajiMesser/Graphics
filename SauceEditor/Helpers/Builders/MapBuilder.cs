using OpenTK;
using SauceEditor.Models.Components;
using SpiceEngine.Maps;
using SpiceEngine.Rendering.Matrices;
using SpiceEngine.Utilities;
using System.IO;

namespace SauceEditor.Helpers.Builders
{
    public static class MapBuilder
    {
        public static void CreateTestProject()
        {
            var project = new Project()
            {
                Name = "MyFirstProject",
                Path = SampleGameProject.Helpers.FilePathHelper.PROJECT_PATH
            };

            project.Maps.Add(new Models.Components.Map()
            {
                Name = "Test Map",
                Path = SampleGameProject.Helpers.FilePathHelper.MAP_PATH
            });

            project.Models.Add(new Model()
            {
                Name = "Bob Lamp",
                Path = SampleGameProject.Helpers.FilePathHelper.BOB_LAMP_MESH_PATH
            });

            project.Materials.Add(new Material()
            {
                Name = "Test Material",
                Path = SampleGameProject.Helpers.FilePathHelper.SHINY_MATERIAL_PATH
            });

            project.Textures.Add(new Texture()
            {
                Name = "Test Texture",
                Path = SampleGameProject.Helpers.FilePathHelper.BRICK_01_D_TEXTURE_PATH,
                TexturePaths = new SpiceEngine.Rendering.Textures.TexturePaths()
                {
                    DiffuseMapFilePath = SampleGameProject.Helpers.FilePathHelper.BRICK_01_D_TEXTURE_PATH,
                    NormalMapFilePath = SampleGameProject.Helpers.FilePathHelper.BRICK_01_N_NORMAL_PATH,
                    SpecularMapFilePath = SampleGameProject.Helpers.FilePathHelper.BRICK_01_S_TEXTURE_PATH,
                    ParallaxMapFilePath = SampleGameProject.Helpers.FilePathHelper.BRICK_01_H_TEXTURE_PATH
                }
            });

            project.Scripts.Add(new Script()
            {
                Name = "Test Script",
                Path = SampleGameProject.Helpers.FilePathHelper.BLOCK_NODE_PATH
            });

            project.Save();

            //project.MapPaths.Add(SampleGameProject.Helpers.FilePathHelper.MAP_PATH);
            //project.ModelPaths.Add(SampleGameProject.Helpers.FilePathHelper.BOB_LAMP_MESH_PATH);
            //project.Save(Path.GetDirectoryName(SampleGameProject.Helpers.FilePathHelper.MAP_PATH) + "\\" + project.Name + Project.FILE_EXTENSION);
        }

        public static SpiceEngine.Maps.Map GenerateModelMap(string filePath)
        {
            var map = new Map3D()
            {
                Camera = new MapCamera()
                {
                    Name = "MainCamera",
                    AttachedActorName = "Player",
                    Position = new Vector3(10.0f, 0.0f, 0.0f),
                    Type = ProjectionTypes.Perspective,
                    ZNear = 0.1f,
                    ZFar = 1000.0f,
                    FieldOfViewY = UnitConversions.ToRadians(45.0f)
                }
            };

            var mapActor = new MapActor()
            {
                Name = "Player",
                Position = Vector3.Zero,
                Rotation = Vector3.Zero,
                Orientation = Vector3.Zero,
                Scale = Vector3.One,
                ModelFilePath = filePath
            };

            map.Actors.Add(mapActor);

            return map;
        }

        public static SpiceEngine.Maps.Map GenerateTextureMap(Texture texture)
        {
            var map = new Map3D()
            {
                Camera = new MapCamera()
                {
                    Name = "MainCamera",
                    //AttachedActorName = "Player",
                    Position = new Vector3(10.0f, 0.0f, 0.0f),
                    Type = ProjectionTypes.Perspective,
                    ZNear = 0.1f,
                    ZFar = 1000.0f,
                    FieldOfViewY = UnitConversions.ToRadians(45.0f)
                }
            };

            var mapBrush = MapBrush.Box(Vector3.Zero, 10.0f, 10.0f, 10.0f);
            mapBrush.TexturesPaths = new SpiceEngine.Rendering.Textures.TexturePaths()
            {
                DiffuseMapFilePath = texture.TexturePaths.DiffuseMapFilePath,
                NormalMapFilePath = texture.TexturePaths.NormalMapFilePath,
                SpecularMapFilePath = texture.TexturePaths.SpecularMapFilePath,
                ParallaxMapFilePath = texture.TexturePaths.ParallaxMapFilePath
            };

            map.Brushes.Add(mapBrush);

            return map;
        }

        public static SpiceEngine.Maps.Map GenerateMaterialMap(Material material)
        {
            var map = new Map3D()
            {
                Camera = new MapCamera()
                {
                    Name = "MainCamera",
                    AttachedActorName = "Player",
                    Position = new Vector3(10.0f, 0.0f, 0.0f),
                    Type = ProjectionTypes.Perspective,
                    ZNear = 0.1f,
                    ZFar = 1000.0f,
                    FieldOfViewY = UnitConversions.ToRadians(45.0f)
                }
            };

            var mapBrush = MapBrush.Sphere(Vector3.Zero, 5.0f);
            //mapBrush.Material = SpiceEngine.Rendering.Materials.Material.LoadFromFile();
            /*mapBrush.TexturesPaths = new SpiceEngine.Rendering.Textures.TexturePaths()
            {
                DiffuseMapFilePath = texture.TexturePaths.DiffuseMapFilePath,
                NormalMapFilePath = texture.TexturePaths.NormalMapFilePath,
                SpecularMapFilePath = texture.TexturePaths.SpecularMapFilePath,
                ParallaxMapFilePath = texture.TexturePaths.ParallaxMapFilePath
            };*/

            map.Brushes.Add(mapBrush);

            return map;
        }
    }
}
