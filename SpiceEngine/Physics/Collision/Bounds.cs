using OpenTK;
using System;
using SpiceEngine.Entities;
using SpiceEngine.Entities.Lights;

namespace SpiceEngine.Physics.Collision
{
    public class Bounds
    {
        public int EntityID { get; }
        public ICollider Collider { get; }

        public Bounds(int entityID, ICollider collider)
        {
            EntityID = entityID;
            Collider = collider;
        }
    }
}
