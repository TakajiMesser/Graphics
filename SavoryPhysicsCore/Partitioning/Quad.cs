using System;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

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
