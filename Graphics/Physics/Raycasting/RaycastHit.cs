﻿using Graphics.Physics.Collision;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Physics.Raycasting
{
    public class RaycastHit
    {
        public Bounds Collider { get; set; }
        public Vector3 Intersection { get; set; }
    }
}
