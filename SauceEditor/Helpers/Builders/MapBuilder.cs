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

            var actor = new MapActor()
            {
                Name = "Player",
                Position = Vector3.Zero,
                Rotation = Vector3.Zero,
                Orientation = Vector3.Zero,
                Scale = Vector3.One,
                ModelFilePath = filePath
            };

            map.Actors.Add(actor);

            return map;
        }
    }
}
