using OpenTK;
using SpiceEngine.Physics.Collision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceEngine.Physics.Shapes
{
    public interface IShape
    {
        float Mass { get; set; }
        float MomentOfInertia { get; }

        ICollider ToCollider(Vector3 position);
    }
}
