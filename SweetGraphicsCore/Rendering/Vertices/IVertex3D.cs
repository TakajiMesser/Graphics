using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Vertices;

namespace SweetGraphicsCore.Vertices
{
    public interface IVertex3D : IVertex
    {
        Vector3 Position { get; }

        IVertex3D Transformed(Transform transform);
    }
}
