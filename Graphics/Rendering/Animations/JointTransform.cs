using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Rendering.Animations
{
    public struct JointTransform
    {
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }

        public JointTransform(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}
