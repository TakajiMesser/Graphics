using OpenTK;
using SpiceEngine.Physics.Bodies;
using System;
using System.Collections.Generic;

namespace SpiceEngine.Physics.Collisions
{
    public interface ICollision
    {
        float PenetrationDepth { get; set; }
        void Resolve();
    }
}
