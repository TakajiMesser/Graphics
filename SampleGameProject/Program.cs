using SampleGameProject.Helpers;
using SampleGameProject.Helpers.Builders;
using SpiceEngine.Maps;
using SpiceEngine.Game;

namespace SampleGameProject
{
    class Program
    {
        static void Main(string[] args)
        {
            ProjectBuilder.CreateMeshOutputFile(SpiceEngineCore.Helpers.FilePathHelper.SPHERE_MESH_PATH, SpiceEngineCore.Helpers.FilePathHelper.SPHERE_OUTPUT_PATH);
            ProjectBuilder.CreateTestProject();
            var map = Map.Load(FilePathHelper.MAP_PATH);

            using (var gameWindow = new GameWindow(map))
            {
                gameWindow.LoadAndRun();
            }
        }
    }
}
