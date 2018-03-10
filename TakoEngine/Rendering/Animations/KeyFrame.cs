using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakoEngine.Rendering.Animations
{
    public class KeyFrame
    {
        public float Time { get; set; }
        public List<JointTransform> Transforms { get; private set; } = new List<JointTransform>();
        //public Vector4 Bezier { get; set; }
    }
}
