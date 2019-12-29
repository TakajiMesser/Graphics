using OpenTK;
using SpiceEngine.Maps;
using TowerWarfare.Builders;
using TowerWarfare.Helpers;
using GameWindow = SpiceEngine.Game.GameWindow;

namespace TowerWarfare
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
                gameWindow.LoadAndRun();
                //gameWindow.Run(60.0, 0.0);
            }
        }
    }
}
