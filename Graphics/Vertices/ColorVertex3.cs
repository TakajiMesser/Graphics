using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Graphics.Helpers;

namespace Graphics
{
    public struct ColorVertex3
    {
        public Vector3 Position { get; private set; }
        public Color4 Color { get; private set; }

        public ColorVertex3(Vector3 position, Color4 color)
        {
            Position = position;
            Color = color;
        }
    }
}
