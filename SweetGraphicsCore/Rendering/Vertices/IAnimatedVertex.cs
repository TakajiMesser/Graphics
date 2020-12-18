using SpiceEngineCore.Geometry.Vectors;
using SpiceEngineCore.Rendering.Vertices;

namespace SweetGraphicsCore.Vertices
{
    public interface IAnimatedVertex : IVertex
    {
        Vector4 BoneIDs { get; }
        Vector4 BoneWeights { get; }
    }
}
