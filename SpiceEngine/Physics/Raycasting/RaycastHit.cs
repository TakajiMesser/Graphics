﻿using OpenTK;
using SpiceEngine.Physics.Collision;

namespace SpiceEngine.Physics.Raycasting
{
    public class RaycastHit
    {
        public int EntityID { get; set; }
        public Vector3 Intersection { get; set; }
    }
}
