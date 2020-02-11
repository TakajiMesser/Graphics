using OpenTK;
using SpiceEngineCore.Physics;
using System;

namespace UmamiPhysicsCore.Collisions
{
    public struct Quad : IPartition
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
            switch (bounds.Partition)
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
