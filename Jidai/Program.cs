using Jidai.Helpers;
using Jidai.Helpers.Builders;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameWindow = SpiceEngine.Game.GameWindow;

namespace Jidai
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
