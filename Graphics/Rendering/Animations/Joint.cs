using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Rendering.Animations
{
    public class Joint
    {
        public int ID { get; private set; }
        public List<Joint> Children { get; private set; } = new List<Joint>();
        public Matrix4 Transform { get; private set; }
    }
}
