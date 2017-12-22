using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Rendering.Animations
{
    public struct KeyFrame
    {
        public float Time { get; set; }
        public float Data { get; set; }
        public Vector4 Bezier { get; set; }
    }
}
