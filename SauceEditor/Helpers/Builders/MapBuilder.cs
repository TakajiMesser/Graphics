using OpenTK;
using OpenTK.Graphics;
using SauceEditor.Models.Components;
using SauceEditorCore.Models.Components;
using SpiceEngine.Maps;
using SpiceEngineCore.Maps;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Textures;
using SpiceEngineCore.Utilities;
using System.Collections.Generic;

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

            project.MapComponents.Add(new MapComponent(SampleGameProject.Helpers.FilePathHelper.MAP_PATH));
            project.ModelComponents.Add(new ModelComponent(SampleGameProject.Helpers.FilePathHelper.BOB_LAMP_MESH_PATH));
            project.MaterialComponents.Add(new MaterialComponent(SampleGameProject.Helpers.FilePathHelper.SHINY_MATERIAL_PATH));
            project.ScriptComponents.Add(new ScriptComponent(SampleGameProject.Helpers.FilePathHelper.BLOCK_NODE_PATH));
            project.TextureComponents.Add(new TextureComponent(SampleGameProject.Helpers.FilePathHelper.BRICK_01_D_TEXTURE_PATH)
            {
                TexturePaths = new SpiceEngineCore.Rendering.Textures.TexturePaths()
                {
                    DiffuseMapFilePath = SampleGameProject.Helpers.FilePathHelper.BRICK_01_D_TEXTURE_PATH,
                    //NormalMapFilePath = SampleGameProject.Helpers.FilePathHelper.BRICK_01_N_NORMAL_PATH,
                    //SpecularMapFilePath = SampleGameProject.Helpers.FilePathHelper.BRICK_01_S_TEXTURE_PATH,
                    //ParallaxMapFilePath = SampleGameProject.Helpers.FilePathHelper.BRICK_01_H_TEXTURE_PATH
                }
            });

            project.Save();

            //project.MapPaths.Add(SampleGameProject.Helpers.FilePathHelper.MAP_PATH);
            //project.ModelPaths.Add(SampleGameProject.Helpers.FilePathHelper.BOB_LAMP_MESH_PATH);
            //project.Save(Path.GetDirectoryName(SampleGameProject.Helpers.FilePathHelper.MAP_PATH) + "\\" + project.Name + Project.FILE_EXTENSION);
        }

        public static MapComponent GenerateModelMap(ModelComponent model)
        {
            var map = new Map3D();

            map.Cameras.Add(new MapCamera()
            {
                Name = "MainCamera",
                AttachedEntityName = "Player",
                Position = new Vector3(10.0f, 0.0f, 0.0f),
                Type = ProjectionTypes.Perspective,
                ZNear = 0.1f,
                ZFar = 1000.0f,
                FieldOfViewY = UnitConversions.ToRadians(45.0f)
            });

            map.Actors.Add(new MapActor()
            {
                Name = "Player",
                Position = Vector3.Zero,
                Rotation = Vector3.Zero,
                Orientation = Vector3.Zero,
                Scale = Vector3.One,
                ModelFilePath = model.Path
            });

            return new MapComponent()
            {
                Name = "Model " + model.Name,
                Map = map
            };
        }

        public static MapComponent GenerateTextureMap(TextureComponent texture)
        {
            var map = new Map3D();

            map.Cameras.Add(new MapCamera()
            {
                Name = "MainCamera",
                //AttachedActorName = "Player",
                Position = new Vector3(0.0f, -20.0f, -20.0f),
                Type = ProjectionTypes.Perspective,
                ZNear = 0.1f,
                ZFar = 1000.0f,
                FieldOfViewY = UnitConversions.ToRadians(45.0f)
            });

            var mapBrush = MapBrush.Box(Vector3.Zero, 10.0f, 10.0f, 10.0f);
            mapBrush.TexturesPaths = new List<TexturePaths>
            {
                new TexturePaths()
                {
                    DiffuseMapFilePath = texture.TexturePaths.DiffuseMapFilePath,
                    NormalMapFilePath = texture.TexturePaths.NormalMapFilePath,
                    //SpecularMapFilePath = texture.TexturePaths.SpecularMapFilePath,
                    //ParallaxMapFilePath = texture.TexturePaths.ParallaxMapFilePath
                }
            };

            map.Brushes.Add(mapBrush);

            map.Lights.Add(new MapLight()
            {
                Radius = 30.0f,
                Intensity = 1.0f,
                Color = Color4.White,
                LightType = MapLight.LightTypes.Point,
                Position = new Vector3(20.0f, 20.0f, 20.0f)
            });

            return new MapComponent()
            {
                Name = "Texture " + texture.Name,
                Map = map
            };
        }

        public static MapComponent GenerateMaterialMap(MaterialComponent material)
        {
            var map = new Map3D();

            map.Cameras.Add(new MapCamera()
            {
                Name = "MainCamera",
                AttachedEntityName = "Player",
                Position = new Vector3(10.0f, 0.0f, 0.0f),
                Type = ProjectionTypes.Perspective,
                ZNear = 0.1f,
                ZFar = 1000.0f,
                FieldOfViewY = UnitConversions.ToRadians(45.0f)
            });

            //var mapBrush = MapBrush.Sphere(Vector3.Zero, 5.0f);
            //mapBrush.Material = SpiceEngine.Rendering.Materials.Material.LoadFromFile();
            /*mapBrush.TexturesPaths = new SpiceEngine.Rendering.Textures.TexturePaths()
            {
                DiffuseMapFilePath = texture.TexturePaths.DiffuseMapFilePath,
                NormalMapFilePath = texture.TexturePaths.NormalMapFilePath,
                SpecularMapFilePath = texture.TexturePaths.SpecularMapFilePath,
                ParallaxMapFilePath = texture.TexturePaths.ParallaxMapFilePath
            };*/

            //map.Brushes.Add(mapBrush);

            return new MapComponent()
            {
                Name = "Material " + material.Name,
                Map = map
            };
        }
    }
}
