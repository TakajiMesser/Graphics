using OpenTK;
using SpiceEngineCore.Physics;
using System.Collections.Generic;
using UmamiPhysicsCore.Bodies;

namespace UmamiPhysicsCore.Collisions
{
    public class Collision2D : ICollision
    {
        private const float PENETRATION_REDUCTION_PERCENTAGE = 0.2f; // usually 20% to 80%
        private const float SLOP = 0.01f; // usually 0.01 to 0.1

        public Body2D FirstBody { get; }
        public Body2D SecondBody { get; }

        public List<Vector2> ContactPoints { get; } = new List<Vector2>();
        public Vector2 ContactNormal { get; set; }
        public float PenetrationDepth { get; set; }

        public bool HasCollision => ContactPoints.Count > 0;

        public Collision2D(Body2D firstBody, Body2D secondBody)
        {
            FirstBody = firstBody;
            SecondBody = secondBody;
        }
    }
}
