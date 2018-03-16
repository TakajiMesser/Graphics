using OpenTK;
using System.Drawing;

namespace TakoEngine.Utilities
{
    public static class OpenTKExtensions
    {
        public static Vector2 ToVector2(this Point point)
        {
            return new Vector2(point.X, point.Y);
        }
    }
}
