using OpenTK;
using SampleGameProject.Helpers;
using SampleGameProject.Helpers.Builders;
using SpiceEngine.Maps;
using GameWindow = SpiceEngine.Game.GameWindow;

namespace SampleGameProject
{
    class Program
    {
        static void Main(string[] args)
        {
            ProjectBuilder.CreateTestProject();
            var map = Map.Load(FilePathHelper.MAP_PATH);

            using (var gameWindow = new GameWindow(map))
            {
                gameWindow.VSync = VSyncMode.Adaptive;
                gameWindow.Run(60.0, 0.0);
                //gameWindow.LoadMap(map);
            }
        }
    }
}
