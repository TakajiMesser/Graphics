using GraphicsTest.Helpers;
using GraphicsTest.Helpers.Builders;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameWindow = Graphics.GameObjects.GameWindow;

namespace GraphicsTest
{
    class Program
    {
        static void Main(string[] args)
        {
            MapBuilder.CreateTestMap();

            using (var game = new GameWindow(FilePathHelper.MAP_PATH))
            {
                game.VSync = VSyncMode.Adaptive;
                game.Run(60.0, 0.0);
            }
        }
    }
}
