using System;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SavoryPhysicsCore.Partitioning
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
