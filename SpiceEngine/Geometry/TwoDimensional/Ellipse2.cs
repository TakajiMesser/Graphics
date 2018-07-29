using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Geometry.TwoDimensional
{
    public class Ellipse2
    {
        public double RadiusA { get; set; }
        public double RadiusB { get; set; }

        public double Area => RadiusA * RadiusB * Math.PI;
    }
}
