using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Lighting
{
    public struct Light
    {
        public Vector3 Position { get; set; }
        public float Radius { get; set; }
        public Vector3 Color { get; set; }
        public float Intensity { get; set; }
    }
}
