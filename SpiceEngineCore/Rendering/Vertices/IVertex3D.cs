using OpenTK;
using SpiceEngineCore.Rendering.Matrices;

namespace SpiceEngineCore.Rendering.Vertices
{
    public interface IVertex3D : IVertex
    {
        Vector3 Position { get; }
        IVertex3D Transformed(Transform transform);
    }
}
