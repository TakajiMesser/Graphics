using OpenTK;

namespace SpiceEngine.Rendering.Vertices
{
    public interface IVertex3D : IVertex
    {
        Vector3 Position { get; }
        IVertex3D Transformed(Matrix4 matrix);
    }
}
