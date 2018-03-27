using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = System.Windows.Media.Color;

namespace SauceEditor.Utilities
{
    public static class UnitConversions
    {
        public static Color ToColor(this Vector4 vector) => Color.FromScRgb(vector.Z, vector.X, vector.Y, vector.Z);

        public static Vector4 ToVector4(this Color color) => new Vector4(color.ScR, color.ScG, color.ScB, color.ScA);
    }
}
