using OpenTK;
using SpiceEngine.Physics.Collision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceEngine.Physics.Shapes
{
    public abstract class Shape3D : IShape
    {
        public abstract float Mass { get; set; }
        public abstract float MomentOfInertia { get; }

        public abstract ICollider ToCollider(Vector3 position);
        public abstract Vector3 GetFurthestPoint(Vector3 position, Vector3 direction);
        public abstract bool CollidesWith(Vector3 position, Vector3 point);

        public static bool Collides(Vector3 positionA, Shape3D shapeA, Vector3 positionB, Shape3D shapeB)
        {
            switch (shapeA)
            {
                case Box boxA when shapeB is Box boxB:
                    return Collides(positionA, boxA, positionB, boxB);
                case Box boxA when shapeB is Sphere sphereB:
                    return Collides(positionA, boxA, positionB, sphereB);
                case Sphere sphereA when shapeB is Box boxB:
                    return Collides(positionB, boxB, positionA, sphereA);
                case Sphere sphereA when shapeB is Sphere sphereB:
                    return Collides(positionA, sphereA, positionB, sphereB);
            }

            throw new NotImplementedException();
        }

        private static bool Collides(Vector3 positionA, Box boxA, Vector3 positionB, Box boxB) =>
            positionA.X - boxA.Width / 2.0f < positionB.X + boxB.Width / 2.0f
            && positionA.X + boxA.Width / 2.0f > positionB.X - boxB.Width / 2.0f
            && positionA.Y - boxA.Height / 2.0f < positionB.Y + boxB.Height / 2.0f
            && positionA.Y + boxA.Height / 2.0f > positionB.Y - boxB.Height / 2.0f
            && positionA.Z - boxA.Depth / 2.0f < positionB.Z + boxB.Depth / 2.0f
            && positionA.Z + boxA.Depth / 2.0f > positionB.Z - boxB.Depth / 2.0f;

        private static bool Collides(Vector3 positionA, Sphere sphereA, Vector3 positionB, Sphere sphereB)
        {
            var distanceSquared = Math.Pow(positionA.X - positionB.X, 2.0f) + Math.Pow(positionA.Y - positionB.Y, 2.0f) + Math.Pow(positionA.Z - positionB.Z, 2.0f);
            return distanceSquared < Math.Pow(sphereA.Radius + sphereB.Radius, 2.0f);
        }

        private static bool Collides(Vector3 positionA, Box boxA, Vector3 positionB, Sphere sphereB)
        {
            var closestX = (positionB.X > positionA.X + boxA.Width / 2.0f)
                ? positionA.X + boxA.Width / 2.0f
                : (positionB.X < positionA.X - boxA.Width / 2.0f)
                    ? positionA.X - boxA.Width / 2.0f
                    : positionB.X;

            var closestY = (positionB.Y > positionA.Y + boxA.Height / 2.0f)
                ? positionA.Y + boxA.Height / 2.0f
                : (positionB.Y < positionA.Y - boxA.Height / 2.0f)
                    ? positionA.Y - boxA.Height / 2.0f
                    : positionB.Y;

            var closestZ = (positionB.Z > positionA.Z + boxA.Depth / 2.0f)
                ? positionA.Z + boxA.Depth / 2.0f
                : (positionB.Z < positionA.Z - boxA.Depth / 2.0f)
                    ? positionA.Z - boxA.Depth / 2.0f
                    : positionB.Z;

            var distanceSquared = Math.Pow(positionB.X - closestX, 2) + Math.Pow(positionB.Y - closestY, 2) + Math.Pow(positionB.Z - closestZ, 2);
            return distanceSquared < Math.Pow(sphereB.Radius, 2);
        }
    }
}
