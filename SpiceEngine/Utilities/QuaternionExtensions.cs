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
            // According to Quaternion.FromEulerAngles()...
            // X = pitch (attitude), Y = yaw (heading), Z = roll (bank)
            // According to wikipedia...
            // q0 = w, q1 = x, q2 = y, q3 = z
            // X = roll (bank/phi), Y = pitch (attitude/theta), Z = yaw (heading/psi)

            return ToAlternateEulerAngles(quaternion);

            var lockValue = quaternion.X * quaternion.Y + quaternion.Z * quaternion.W;

            float heading;
            float attitude;
            float bank;

            if (lockValue > 0.5f - MathExtensions.EPSILON)
            {
                heading = 2.0f * (float)Math.Atan2(quaternion.X, quaternion.W);
                attitude = (float)Math.PI / 2.0f;
                bank = 0.0f;
            }
            else if (lockValue < -0.5f + MathExtensions.EPSILON)
            {
                heading = -2.0f * (float)Math.Atan2(quaternion.X, quaternion.W);
                attitude = -(float)Math.PI / 2.0f;
                bank = 0.0f;
            }
            else
            {
                heading = (float)Math.Atan2(2.0f * (quaternion.Y * quaternion.W - quaternion.X * quaternion.Z), 1.0f - 2.0f * (quaternion.Y * quaternion.Y - quaternion.Z * quaternion.Z));
                attitude = (float)Math.Asin(2.0f * lockValue);
                bank = (float)Math.Atan2(2.0f * (quaternion.X * quaternion.W - quaternion.Y * quaternion.Z), 1.0f - 2.0f * (quaternion.X * quaternion.X - quaternion.Z * quaternion.Z));
            }

            // To their standards, bank is X, heading is Y, and attitude is Z
            // To our standards, attitude is X, heading is Y, and bank is Z

            return new Vector3()
            {
                X = attitude,
                Y = heading,
                Z = bank
            };

            //var lockValue = 2.0f * (quaternion.Y * quaternion.W + quaternion.X * quaternion.Z);
            //var lockValue = -(quaternion.X * quaternion.X) - (quaternion.Y * quaternion.Y) + quaternion.Z * quaternion.Z + quaternion.W * quaternion.W;

            //if (lockValue == 1.0f || lockValue == -1.0f)
            if (lockValue == 0.0f)
            {
                // This will cause gimbal-lock, since arcsin of 1.0f or -1.0f will result in Pi/2 or -Pi/2
                // TODO - Handle this situation...
                throw new Exception("Gimbal lock sucks :(");
            }

            // This is assuming that phi = X, theta = Y, and psi = Z
            var phi = (float)Math.Atan2(quaternion.Z * quaternion.W + quaternion.X * quaternion.Y, 0.5f - (quaternion.Y * quaternion.Y + quaternion.Z * quaternion.Z));
            var theta = (float)Math.Asin(-2.0f * (quaternion.Y * quaternion.W - quaternion.X * quaternion.Z));
            //var theta = (float)Math.Asin(lockValue);
            var psi = (float)Math.Atan2(quaternion.Y * quaternion.Z + quaternion.X * quaternion.W, 0.5f - (quaternion.Z * quaternion.Z + quaternion.W * quaternion.W));
            //var phi = (float)Math.Atan2(quaternion.X * quaternion.Z + quaternion.Y * quaternion.W, quaternion.X * quaternion.W - quaternion.Y * quaternion.Z);
            //var theta = (float)Math.Acos(lockValue);
            //var psi = (float)Math.Atan2(quaternion.X * quaternion.Z - quaternion.Y * quaternion.W, quaternion.Y * quaternion.Z + quaternion.X * quaternion.W);

            // This causes issues, since the identity quaternion (0, 0, 0, 1) causes psi of pi, when it should be zero!
            //var y = quaternion.Y * quaternion.Z + quaternion.X * quaternion.W;
            //var x = 0.5f - (quaternion.Z * quaternion.Z + quaternion.W * quaternion.W);

            // Atan2 is from -pi to +pi and Asin is from -pi/2 to +pi/2
            phi = phi.IsSignificant() ? phi : 0.0f;
            theta = theta.IsSignificant() ? theta : 0.0f;
            psi = psi.IsSignificant() ? psi : 0.0f;

            var eulerAngles = new Vector3()
            {
                X = phi,
                Y = theta,
                Z = psi
            };

            return eulerAngles;
        }

        private static Vector3 ToAlternateEulerAngles(this Quaternion quaternion)
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
