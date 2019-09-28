using System;
using System.Runtime.InteropServices;

namespace SpiceEngine.Utilities
{
    public static class UnitConversions
    {
        /// <summary>
        /// Converts from degrees to radians
        /// </summary>
        /// <param name="degrees">Degrees value to convert</param>
        /// <returns>Converted value in radians</returns>
        public static double ToRadians(double degrees) => degrees * Math.PI / 180;
        public static float ToRadians(float degrees) => degrees * MathExtensions.PI / 180;

        /// <summary>
        /// Converts from radians to degrees
        /// </summary>
        /// <param name="radians">Radians value to convert</param>
        /// <returns>Converted value in degrees</returns>
        public static double ToDegrees(double radians) => radians * 180 / Math.PI;
        public static float ToDegrees(float radians) => radians * 180 / MathExtensions.PI;

        /// <summary>
        /// Calculates the size of an unmanaged type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>The size, in bytes</returns>
        public static int SizeOf<T>() where T : struct => Marshal.SizeOf(typeof(T));
        public static int SizeOf(Type type) => Marshal.SizeOf(type);
    }
}
