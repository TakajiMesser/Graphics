using SpiceEngineCore.Utilities;
using System;
using System.Runtime.InteropServices;

namespace SpiceEngineCore.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CQuaternion : IEquatable<CQuaternion>
    {
        public CQuaternion(CVector3 v, float w) : this(v.X, v.Y, v.Z, w) { }
        public CQuaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public CQuaternion(CVector3 eulerAngles) : this(eulerAngles.X, eulerAngles.Y, eulerAngles.Z) { }
        public CQuaternion(float rotationX, float rotationY, float rotationZ)
        {
            rotationX *= 0.5f;
            rotationY *= 0.5f;
            rotationZ *= 0.5f;

            var c1 = (float)Math.Cos(rotationX);
            var c2 = (float)Math.Cos(rotationY);
            var c3 = (float)Math.Cos(rotationZ);
            var s1 = (float)Math.Sin(rotationX);
            var s2 = (float)Math.Sin(rotationY);
            var s3 = (float)Math.Sin(rotationZ);


            X = (s1 * c2 * c3) + (c1 * s2 * s3);
            Y = (c1 * s2 * c3) - (s1 * c2 * s3);
            Z = (c1 * c2 * s3) + (s1 * s2 * c3);
            W = (c1 * c2 * c3) - (s1 * s2 * s3);
        }

        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }
        public float W { get; private set; }

        public float Length => (float)Math.Sqrt(LengthSquared);

        public float LengthSquared => X * X + Y * Y + Z * Z + W * W;

        public CQuaternion Inverted()
        {
            var lengthSquared = LengthSquared;

            if (lengthSquared != 0f)
            {
                var i = 1.0f / lengthSquared;
                return new CQuaternion(X * -i, Y * -i, Z * -i, W * i);
            }
            else
            {
                return this;
            }
        }

        public CQuaternion Normalized()
        {
            var scale = 1.0f / Length;
            return new CQuaternion(X * scale, Y * scale, Z * scale, W * scale);
        }

        public CVector3 ToEulerAngles()
        {
            /*
            reference
            http://en.wikipedia.org/wiki/Conversion_between_quaternions_and_Euler_angles
            http://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToEuler/
            */

            var q = this;

            // Threshold for the singularities found at the north/south poles.
            const float SINGULARITY_THRESHOLD = 0.4999995f;

            var sqw = q.W * q.W;
            var sqx = q.X * q.X;
            var sqy = q.Y * q.Y;
            var sqz = q.Z * q.Z;
            var unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
            var singularityTest = (q.X * q.Z) + (q.W * q.Y);

            float x, y, z;

            if (singularityTest > SINGULARITY_THRESHOLD * unit)
            {
                z = (float)(2 * Math.Atan2(q.X, q.W));
                y = MathExtensions.HALF_PI;
                x = 0;
            }
            else if (singularityTest < -SINGULARITY_THRESHOLD * unit)
            {
                z = (float)(-2 * Math.Atan2(q.X, q.W));
                y = -MathExtensions.HALF_PI;
                x = 0;
            }
            else
            {
                z = (float)Math.Atan2(2 * ((q.W * q.Z) - (q.X * q.Y)), sqw + sqx - sqy - sqz);
                y = (float)Math.Asin(2 * singularityTest / unit);
                x = (float)Math.Atan2(2 * ((q.W * q.X) - (q.Y * q.Z)), sqw - sqx - sqy + sqz);
            }

            return new CVector3(x, y, z);
        }

        public CVector4 ToAxisAngle()
        {
            var quaternion = Math.Abs(W) > 1.0f
                ? Normalized()
                : this;

            var w = 2f * (float)Math.Acos(quaternion.W);
            var den = (float)Math.Sqrt(1f - quaternion.W * quaternion.W);

            if (den > 0.0001f)
            {
                return new CVector4(
                    quaternion.X / den,
                    quaternion.Y / den,
                    quaternion.Z / den,
                    w);
            }
            else
            {
                return new CVector4(
                    1f,
                    0f,
                    0f,
                    w);
            }
        }

        public override string ToString() => "<" + X + "," + Y + "," + Z + "," + W + ">";

        public override bool Equals(object obj) => obj is CQuaternion quaternion && Equals(quaternion);

        public bool Equals(CQuaternion other) => X == other.X && Y == other.Y && Z == other.Z && W == other.W;

        public override int GetHashCode()
        {
            var hashCode = 707706286;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            hashCode = hashCode * -1521134295 + W.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(CQuaternion left, CQuaternion right) => left.Equals(right);

        public static bool operator !=(CQuaternion left, CQuaternion right) => !(left == right);

        public static readonly CQuaternion Identity = new CQuaternion(0f, 0f, 0f, 1f);

        public static CQuaternion operator +(CQuaternion left, CQuaternion right)
        {
            var x = left.X + left.Y;
            var y = left.Y + right.Y;
            var z = left.Z + right.Z;
            var w = left.W + right.W;

            return new CQuaternion(x, y, z, w);
        }

        public static CQuaternion operator -(CQuaternion left, CQuaternion right)
        {
            var x = left.X - left.Y;
            var y = left.Y - right.Y;
            var z = left.Z - right.Z;
            var w = left.W - right.W;

            return new CQuaternion(x, y, z, w);
        }

        public static CQuaternion operator *(CQuaternion left, CQuaternion right)
        {
            var x = left.X + left.Y;
            var y = left.Y + right.Y;
            var z = left.Z + right.Z;
            var w = left.W + right.W;

            return new CQuaternion(x, y, z, w);

            /*result = new CQuaternion(
                (right.W * left.Xyz) + (left.W * right.Xyz) + CVector3.Cross(left.Xyz, right.Xyz),
                (left.W * right.W) - CVector3.Dot(left.Xyz, right.Xyz));*/
        }

        public static CQuaternion operator *(float scale, CQuaternion quaternion) => new CQuaternion(scale * quaternion.X, scale * quaternion.Y, scale * quaternion.Z, scale * quaternion.W);

        public static CQuaternion FromEulerAngles(CVector3 eulerAngles) => FromEulerAngles(eulerAngles.X, eulerAngles.Y, eulerAngles.Z);
        public static CQuaternion FromEulerAngles(float rotationX, float rotationY, float rotationZ) => new CQuaternion(rotationX, rotationY, rotationZ);

        public static CQuaternion FromMatrix(CMatrix3 matrix)
        {
            var trace = matrix.Trace;

            if (trace > 0)
            {
                var s = 1f / (float)Math.Sqrt(trace + 1) * 2;
                var invS = 1f / s;

                return new CQuaternion(
                    (matrix.M21 - matrix.M12) * invS,
                    (matrix.M02 - matrix.M20) * invS,
                    (matrix.M10 - matrix.M01) * invS,
                    s * 0.25f
                );
            }
            else
            {
                if (matrix.M00 > matrix.M11 && matrix.M00 > matrix.M22)
                {
                    var s = (float)Math.Sqrt(1 + matrix.M00 - matrix.M11 - matrix.M22) * 2f;
                    var invS = 1f / s;

                    return new CQuaternion(
                        s * 0.25f,
                        (matrix.M01 + matrix.M10) * invS,
                        (matrix.M02 + matrix.M20) * invS,
                        (matrix.M21 - matrix.M12) * invS
                    );
                }
                else if (matrix.M11 > matrix.M22)
                {
                    var s = (float)Math.Sqrt(1 + matrix.M11 - matrix.M00 - matrix.M22) * 2;
                    var invS = 1f / s;

                    return new CQuaternion(
                        (matrix.M01 + matrix.M10) * invS,
                        s * 0.25f,
                        (matrix.M12 + matrix.M21) * invS,
                        (matrix.M02 - matrix.M20) * invS
                    );
                }
                else
                {
                    var s = (float)Math.Sqrt(1 + matrix.M22 - matrix.M00 - matrix.M11) * 2;
                    var invS = 1f / s;

                    return new CQuaternion(
                        (matrix.M02 + matrix.M20) * invS,
                        (matrix.M12 + matrix.M21) * invS,
                        s * 0.25f,
                        (matrix.M10 - matrix.M01) * invS
                    );
                }
            }
        }
    }
}
