using OpenTK;
using SpiceEngineCore.Rendering.Shaders;
using System;
using System.Collections.Generic;
using System.IO;

namespace SpiceEngineCore.Rendering.UserInterfaces.Attributes
{
    public struct Size
    {
        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; set; }
        public int Height { get; set; }
    }
}
