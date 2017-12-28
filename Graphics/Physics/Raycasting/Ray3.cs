using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Physics.Raycasting
{
    public struct Ray3
    {
        public Vector3 Origin { get; set; }
        public Vector3 Direction { get; set; }

        public Ray3(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction;
        }
    }
}
