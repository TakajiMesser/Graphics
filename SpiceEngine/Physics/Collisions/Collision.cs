using OpenTK;
using System;

namespace SpiceEngine.Physics.Collisions
{
    public class Collision
    {
        public CollisionPair CollisionPair { get; }
        public Vector3 Point { get; }

        public Collision(CollisionPair collisionPair)
        {
            CollisionPair = collisionPair;
        }
    }
}
