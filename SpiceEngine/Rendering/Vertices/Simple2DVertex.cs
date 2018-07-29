using OpenTK;
using System.Runtime.InteropServices;

namespace SpiceEngine.Rendering.Vertices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Simple2DVertex : IVertex
    {
        public Vector2 Position;

        public Simple2DVertex(Vector2 position)
        {
            Position = position;
        }
    }
}
