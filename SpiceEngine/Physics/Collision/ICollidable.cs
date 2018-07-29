using OpenTK;
using System;
using SpiceEngine.Entities;
using SpiceEngine.Entities.Lights;

namespace SpiceEngine.Physics.Collision
{
    public interface ICollidable
    {
        Bounds Bounds { get; set; }
        bool HasCollision { get; set; }
    }
}
