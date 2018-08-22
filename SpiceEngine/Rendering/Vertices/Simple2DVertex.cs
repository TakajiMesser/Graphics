using OpenTK;
using System.Runtime.InteropServices;

namespace SpiceEngine.Rendering.Vertices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Simple2DVertex : IVertex2D
    {
        public Vector2 Position { get; private set; }

        public Simple2DVertex(Vector2 position)
        {
            Position = position;
        }
    }
}
