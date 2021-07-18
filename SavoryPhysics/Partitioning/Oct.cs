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
    public struct Oct : IPartition
    {
        public Vector3 Min { get; set; }
        public Vector3 Max { get; set; }

        public float Length => (Max - Min).Length;

        public Oct(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }

        public bool CanContain(Bounds bounds)
        {
            switch (bounds.Partition)
            {
                case Oct oct:
                    return Min.X < oct.Min.X
                        && Min.Y < oct.Min.Y
                        && Min.Z < oct.Min.Z
                        && Max.X > oct.Max.X
                        && Max.Y > oct.Max.Y
                        && Max.Z > oct.Max.Z;
            }

            throw new NotImplementedException();
        }
    }
}
