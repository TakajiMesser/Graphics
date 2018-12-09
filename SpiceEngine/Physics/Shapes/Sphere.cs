﻿using OpenTK;
using SpiceEngine.Physics.Collision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceEngine.Physics.Shapes
{
    public class Sphere : Shape3D
    {
        public float Radius { get; }

        public override Vector3 Center { get; }
        public override float Mass { get; set; }
        public override float MomentOfInertia { get; }

        public Sphere(IEnumerable<Vector3> vertices)
        {
            var maxDistanceSquared = vertices.Select(v => v.LengthSquared).Max();
            Radius = (float)Math.Sqrt(maxDistanceSquared);
        }

        public override ICollider ToCollider(Vector3 position)
        {
            var min = new Vector3(position.X - Center.X - Radius, position.Y - Center.Y - Radius, position.Z - Center.Z - Radius);
            var max = new Vector3(position.X - Center.X + Radius, position.Y - Center.Y + Radius, position.Z - Center.Z + Radius);

            return new Oct(min, max);
        }

        public override Vector3 GetFurthestPoint(Vector3 position, Vector3 direction)
        {
            return position - Center + (direction.Normalized() * Radius);
        }

        public override bool CollidesWith(Vector3 position, Vector3 point)
        {
            var distanceSquared = Math.Pow(point.X - position.X - Center.X, 2.0f) + Math.Pow(point.Y - position.Y - Center.Y, 2.0f) + Math.Pow(point.Z - position.Z - Center.Z, 2.0f);
            return distanceSquared < Math.Pow(Radius, 2.0f);
        }
    }
}
