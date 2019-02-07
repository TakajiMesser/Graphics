using SpiceEngine.Entities;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;
using System.Collections.Generic;

namespace SpiceEngine.Rendering.Batches
{
    public interface IBatch
    {
        int EntityID { get; }
        IEnumerable<IVertex3D> Vertices { get; }
        void Load();
        void Draw(IEntityProvider entityProvider, ShaderProgram shaderProgram, TextureManager textureManager = null);
        IBatch Duplicate(int entityID);
    }
}
