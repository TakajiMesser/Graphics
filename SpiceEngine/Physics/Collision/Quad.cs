using OpenTK;
using System;

namespace SpiceEngine.Physics.Collision
{
    public struct Quad : ICollider
    {
        public Vector2 Min { get; }
        public Vector2 Max { get; }

        public float Length => (Max - Min).Length;

        public Quad(Vector2 min, Vector2 max)
        {
            Min = min;
            Max = max;
        }

        public bool CanContain(Bounds bounds)
        {
            switch (bounds.Collider)
            {
                case Quad quad:
                    return Min.X < quad.Min.X
                        && Min.Y < quad.Min.Y
                        && Max.X > quad.Max.X
                        && Max.Y > quad.Max.Y;
            }

            throw new NotImplementedException();
        }
    }
}
