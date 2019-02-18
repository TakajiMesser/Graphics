using OpenTK;
using SpiceEngine.Physics.Collisions;
using System;

namespace SpiceEngine.Physics.Shapes
{
    public abstract class Shape3D : IShape
    {
        public abstract Vector3 Center { get; }
        public abstract float Mass { get; set; }
        public abstract float MomentOfInertia { get; }

        public abstract IPartition ToCollider(Vector3 position);
        public abstract Vector3 GetFurthestPoint(Vector3 position, Vector3 direction);
        public abstract bool CollidesWith(Vector3 position, Vector3 point);
        public abstract IShape Duplicate();

        public static bool Collides(Vector3 positionA, Shape3D shapeA, Vector3 positionB, Shape3D shapeB)
        {
            switch (shapeA)
            {
                case Box boxA when shapeB is Box boxB:
                    return Collides(positionA - boxA.Center, boxA, positionB - boxB.Center, boxB);
                case Box boxA when shapeB is Sphere sphereB:
                    return Collides(positionA - boxA.Center, boxA, positionB - sphereB.Center, sphereB);
                case Sphere sphereA when shapeB is Box boxB:
                    return Collides(positionB - sphereA.Center, boxB, positionA - boxB.Center, sphereA);
                case Sphere sphereA when shapeB is Sphere sphereB:
                    return Collides(positionA - sphereA.Center, sphereA, positionB - sphereB.Center, sphereB);
            }

            throw new NotImplementedException();
        }

        private static bool Collides(Vector3 positionA, Box boxA, Vector3 positionB, Box boxB)
        {
            bool collides = positionA.X - boxA.Width / 2.0f < positionB.X + boxB.Width / 2.0f
                && positionA.X + boxA.Width / 2.0f > positionB.X - boxB.Width / 2.0f
                && positionA.Y - boxA.Height / 2.0f < positionB.Y + boxB.Height / 2.0f
                && positionA.Y + boxA.Height / 2.0f > positionB.Y - boxB.Height / 2.0f
                && positionA.Z - boxA.Depth / 2.0f < positionB.Z + boxB.Depth / 2.0f
                && positionA.Z + boxA.Depth / 2.0f > positionB.Z - boxB.Depth / 2.0f;

            if (collides)
            {
                var minXA = positionA.X - boxA.Width / 2.0f;
                var maxXA = positionA.X + boxA.Width / 2.0f;
                var minYA = positionA.Y - boxA.Height / 2.0f;
                var maxYA = positionA.Y + boxA.Height / 2.0f;
                var minZA = positionA.Z - boxA.Depth / 2.0f;
                var maxZA = positionA.Z + boxA.Depth / 2.0f;

                var minXB = positionB.X - boxB.Width / 2.0f;
                var maxXB = positionB.X + boxB.Width / 2.0f;
                var minYB = positionB.Y - boxB.Height / 2.0f;
                var maxYB = positionB.Y + boxB.Height / 2.0f;
                var minZB = positionB.Z - boxB.Depth / 2.0f;
                var maxZB = positionB.Z + boxB.Depth / 2.0f;
            }

            return collides;
        }
            

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
