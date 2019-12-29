using OpenTK;
using SpiceEngineCore.Rendering.Shaders;
using System;
using System.Collections.Generic;
using System.IO;

namespace SpiceEngineCore.Rendering.UserInterfaces.Attributes
{
    public struct Position
    {
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; private set; }
        public int Y { get; private set; }
    }
}
