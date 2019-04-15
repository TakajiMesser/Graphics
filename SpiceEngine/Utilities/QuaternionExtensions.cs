using OpenTK;
using System;

namespace SpiceEngine.Utilities
{
    public static class QuaternionExtensions
    {
        public static float AngleBetween(this Quaternion quaternionA, Quaternion quaternionB)
        {
            var angle = Math.Acos((quaternionA * quaternionB.Inverted()).W);
            if (angle > Math.PI)
            {
                angle = 2.0 * Math.PI - angle;
            }

            return (float)angle;
        }

        public static bool IsSignificant(this Quaternion quaternion) => quaternion.Xyz.IsSignificant()
            || quaternion.W >= MathExtensions.EPSILON
            || quaternion.W <= -MathExtensions.EPSILON;

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
                pitch = 0.5f * (float)Math.PI;
                roll = 0.0f;
            }
            else if (testValue < -0.499f * correctionFactor)
            {
                // Singularity at south pole
                yaw = -2.0f * (float)Math.Atan2(quaternion.X, quaternion.W);
                pitch = -0.5f * (float)Math.PI;
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
