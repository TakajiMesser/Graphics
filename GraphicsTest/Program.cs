﻿using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameWindow = MappingTool.GameWindow;

namespace GraphicsTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var game = new GameWindow(Graphics.Helpers.FilePathHelper.MAP_PATH))
            {
                game.VSync = VSyncMode.Adaptive;
                game.Run(60.0, 0.0);
            }
        }
    }
}
