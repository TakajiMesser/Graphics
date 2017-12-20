using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Utilities
{
    public static class MathExtensions
    {
        public static bool IsBetween(this float value, float valueA, float valueB) => (value > valueA && value < valueB) || (value < valueA && value > valueB);
    }
}
