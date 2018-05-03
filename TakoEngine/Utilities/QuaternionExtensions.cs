using OpenTK;
using System;

namespace TakoEngine.Utilities
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
    }
}
