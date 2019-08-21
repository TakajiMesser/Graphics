using OpenTK;
using SpiceEngine.Rendering.Matrices;

namespace SpiceEngine.Rendering.Vertices
{
    public interface IVertex3D : IVertex
    {
        Vector3 Position { get; }
        IVertex3D Transformed(Transform transform);
    }
}
