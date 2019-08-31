using OpenTK;
using System;

namespace SpiceEngine.Utilities
{
    public static class QuaternionExtensions
    {
        public static float AngleBetween(this Quaternion quaternionA, Quaternion quaternionB)
        {
            var angle = (float)Math.Acos((quaternionA * quaternionB.Inverted()).W);
            if (angle > MathExtensions.PI)
            {
                angle = 2.0f * MathExtensions.PI - angle;
            }

            return angle;
        }

        public static bool IsSignificant(this Quaternion quaternion) => (quaternion.X - 1.0f).IsSignificant()
            || (quaternion.Y - 1.0f).IsSignificant()
            || (quaternion.Z - 1.0f).IsSignificant()
            || (quaternion.W - 1.0f).IsSignificant();

        public static Vector3 ApplyTo(this Quaternion quaternion, Vector3 vector) => (new Vector4(vector, 1.0f) * Matrix4.CreateFromQuaternion(quaternion)).Xyz;

        /*public static bool IsSignificant(this Quaternion quaternion) => quaternion.Xyz.IsSignificant()
            || quaternion.W >= MathExtensions.EPSILON
            || quaternion.W <= -MathExtensions.EPSILON;*/

        // From http://www.opengl-tutorial.org/intermediate-tutorials/tutorial-17-quaternions/
        public static Quaternion RotationBetween(Vector vectorA, Vector vectorB)
        {
            var normalizedVectorA = vectorA.Normalized();
            var normalizedVectorB = vectorB.Normalized();

            var cosAngle = Vector3.Dot(normalizedVectorA, normalizedVectorB);

            if (cosAngle < -0.999f)
            {
                var rotationAxis = Vector3.Cross(Vector3.UnitZ, normalizedVectorA);
                if (rotationAxis.Length() < 0.01f)
                {
                    rotationAxis = Vector3.Cross(Vector3.UnitX, normalizedVectorA);
                }

                return Quaternion.FromAxisAngle(rotationAxis.Normalized(), MathExtensions.PI);
            }
            else
            {
                var rotationAxis = Vector3.Cross(normalizedVectorA, normalizedVectorB);
                var s = (float)Math.Sqrt((1.0f + cosAngle) * 2.0f);
                var inverseS = 1.0f / s;

                return new Quaternion()
                {
                    X = rotationAxis.X * inverseS,
                    Y = rotationAxis.Y * inverseS,
                    Z = rotationAxis.Z * inverseS,
                    W = s * 0.5f
                };
            }
        }

        public static Quaternion RotationBetween(Vector vectorA, Vector vectorB, Vector3 up)
        {
            var directionRotation = RotationBetween(vectorA, vectorB);
            
            // Recompute up-vector to be perpendicular to vectorB
            var right = Vector3.Cross(vectorB, up);
            up = Vector3.Cross(right, vectorB);

            // If we want to orient the object in a different "up" than the world up, the below "up" should be the world up
            var correctedUp = directionRotation * up;
            var upRotation = RotationBetween(correctedUp, up);

            return upRotation * directionRotation;
        }

        // Using Tait-Bryan rotation convention conversion from this post: https://math.stackexchange.com/questions/687964/getting-euler-tait-bryan-angles-from-quaternion-representation
        public static Vector3 ToEulerAngles(this Quaternion quaternion)
        {
            float pitch;
            float yaw;
            float roll;

            double xSquared = quaternion.X * quaternion.X;
            double ySquared = quaternion.Y * quaternion.Y;
            double zSquared = quaternion.Z * quaternion.Z;
            double wSquared = quaternion.W * quaternion.W;

            double correctionFactor = xSquared + ySquared + zSquared + wSquared;
            double testValue = quaternion.X * quaternion.Y + quaternion.Z * quaternion.W;

            if (testValue > 0.499f * correctionFactor)
            {
                // Singularity at north pole
                yaw = 2.0f * (float)Math.Atan2(quaternion.X, quaternion.W);
                pitch = 0.5f * MathExtensions.PI;
                roll = 0.0f;
            }
            else if (testValue < -0.499f * correctionFactor)
            {
                // Singularity at south pole
                yaw = -2.0f * (float)Math.Atan2(quaternion.X, quaternion.W);
                pitch = -0.5f * MathExtensions.PI;
                roll = 0.0f;
            }
            else
            {
                yaw = (float)Math.Atan2(2.0f * quaternion.Y * quaternion.W - 2.0f * quaternion.X * quaternion.Z, xSquared - ySquared - zSquared + wSquared);
                pitch = (float)Math.Asin(2.0f * testValue / correctionFactor);
                roll = (float)Math.Atan2(2.0f * quaternion.X * quaternion.W - 2.0f * quaternion.Y * quaternion.Z, -xSquared + ySquared - zSquared + wSquared);
            }

            return new Vector3()
            {
                X = pitch,
                Y = yaw,
                Z = roll
            };
        }
    }
}
