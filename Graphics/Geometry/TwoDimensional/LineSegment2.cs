using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Geometry.TwoDimensional
{
    public struct LineSegment2
    {
        public Vector2 Start { get; set; }
        public Vector2 End { get; set; }

        public double Length => Math.Sqrt(Math.Pow(End.X - Start.X, 2) + Math.Pow(End.Y - Start.Y, 2));
    }
}
