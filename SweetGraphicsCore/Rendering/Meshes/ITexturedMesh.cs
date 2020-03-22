using SpiceEngineCore.Rendering.Materials;
using SpiceEngineCore.Rendering.Textures;

namespace SweetGraphicsCore.Rendering.Meshes
{
    public interface ITexturedMesh : IMesh
    {
        Material Material { get; set; }
        TextureMapping? TextureMapping { get; set; }
    }
}
