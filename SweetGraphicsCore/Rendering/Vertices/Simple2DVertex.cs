using SpiceEngineCore.Geometry;
using System.Runtime.InteropServices;

namespace SweetGraphicsCore.Vertices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Simple2DVertex : IVertex2D
    {
        public Simple2DVertex(Vector2 position) => Position = position;

        public Vector2 Position { get; }
    }
}
