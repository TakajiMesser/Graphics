using OpenTK;
using System.Runtime.InteropServices;

namespace TakoEngine.Rendering.Vertices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Simple3DVertex// : IVertex
    {
        public Vector3 Position;// { get; set; }

        public Simple3DVertex(Vector3 position)
        {
            Position = position;
        }
    }
}
