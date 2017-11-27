using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES20;

namespace Graphics
{
    public struct ColorVertex2
    {
        public const int SIZE = (3 + 4) * 4;

        private Vector2 _position;
        private Color4 _color;

        public ColorVertex2(Vector2 position, Color4 color)
        {
            _position = position;
            _color = color;
        }
    }
}
