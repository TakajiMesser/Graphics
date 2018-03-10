using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakoEngine.Outputs
{
    public class Resolution
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public float AspectRatio => (float)Width / Height;

        public Resolution(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
