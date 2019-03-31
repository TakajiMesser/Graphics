using Jidai.Helpers;
using Jidai.Helpers.Builders;
using OpenTK;
using SpiceEngine.Maps;
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
            MapBuilder.GenerateTestMap(FilePathHelper.MAP_PATH);
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
