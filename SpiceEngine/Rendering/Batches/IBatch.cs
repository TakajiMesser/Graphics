using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiceEngine.Rendering.Buffers;
using SpiceEngine.Rendering.Vertices;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Entities;

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
