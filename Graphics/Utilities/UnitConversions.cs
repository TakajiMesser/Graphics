using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Utilities
{
    public static class UnitConversions
    {
        /// <summary>
        /// Converts from degrees to radians
        /// </summary>
        /// <param name="degrees">Degrees value to convert</param>
        /// <returns>Converted value in radians</returns>
        public static double DegreesToRadians(double degrees) => degrees * Math.PI / 180;

        /// <summary>
        /// Converts from radians to degrees
        /// </summary>
        /// <param name="radians">Radians value to convert</param>
        /// <returns>Converted value in degrees</returns>
        public static double RadiansToDegrees(double radians) => radians * 180 / Math.PI;
        public static float RadiansToDegrees(float radians) => (float)(radians * 180 / Math.PI);

        /// <summary>
        /// Calculates the size of an unmanaged type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>The size, in bytes</returns>
        public static int SizeOf<T>() where T : struct => Marshal.SizeOf(typeof(T));
    }
}
