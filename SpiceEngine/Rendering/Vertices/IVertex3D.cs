using OpenTK;

namespace SpiceEngine.Rendering.Vertices
{
    public interface IVertex3D : IVertex
    {
        Vector3 Position { get; }
    }
}
