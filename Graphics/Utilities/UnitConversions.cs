using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
