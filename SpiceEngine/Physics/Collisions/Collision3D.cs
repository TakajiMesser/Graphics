using OpenTK;
using SpiceEngine.Physics.Bodies;
using System.Collections.Generic;

namespace SpiceEngine.Physics.Collisions
{
    public class Collision3D : ICollision
    {
        private const float PENETRATION_REDUCTION_PERCENTAGE = 0.4f; // usually 20% to 80%
        private const float SLOP = 0.05f; // usually 0.01 to 0.1

        public Body3D FirstBody { get; }
        public Body3D SecondBody { get; }

        public Vector3 ContactPoint { get; set; }
        public Vector3 ContactNormal { get; set; }
        public float PenetrationDepth { get; set; }

        public bool HasCollision { get; set; }

        public Collision3D(Body3D firstBody, Body3D secondBody)
        {
            FirstBody = firstBody;
            SecondBody = secondBody;
        }
    }
}
