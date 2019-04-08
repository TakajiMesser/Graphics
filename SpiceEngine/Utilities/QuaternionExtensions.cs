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
            var lockValue = -2.0f * (quaternion.Y * quaternion.W + quaternion.X * quaternion.Z);

            if (lockValue == 1.0f || lockValue == -1.0f)
            {
                // This will cause gimbal-lock, since arcsin of 1.0f or -1.0f will result in Pi/2 or -Pi/2
                // TODO - Handle this situation...
            }

            // This is assuming that phi = X, theta = Y, and psi = Z
            var phi = (float)Math.Atan2(quaternion.Z * quaternion.W + quaternion.X * quaternion.Y, 0.5f - (quaternion.Y * quaternion.Y + quaternion.Z * quaternion.Z));
            var theta = (float)Math.Asin(lockValue);
            var psi = (float)Math.Atan2(quaternion.Y * quaternion.Z + quaternion.X * quaternion.W, 0.5f - (quaternion.Z * quaternion.Z + quaternion.W * quaternion.W));

            var eulerAngles = new Vector3()
            {
                X = phi,
                Y = theta,
                Z = psi
            };

            return eulerAngles;
        }
    }
}
