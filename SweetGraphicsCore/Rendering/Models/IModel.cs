using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Shaders;
using SweetGraphicsCore.Rendering.Meshes;
using System.Collections.Generic;

namespace SweetGraphicsCore.Rendering.Models
{
    public interface IModel : IRenderable
    {
        List<IMesh> Meshes { get; }

        void Add(IMesh mesh);
        void SetUniforms(ShaderProgram shaderProgram, int meshIndex);
        IModel Duplicate();
    }
}
