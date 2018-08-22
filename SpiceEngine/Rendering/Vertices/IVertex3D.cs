using OpenTK;
using OpenTK.Graphics;

namespace SpiceEngine.Rendering.Vertices
{
    public interface IVertex3D : IVertex
    {
        Vector3 Position { get; }
    }
}
