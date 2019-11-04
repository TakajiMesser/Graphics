using SpiceEngineCore.Rendering.Meshes;
using System.Collections.Generic;

namespace SpiceEngineCore.Rendering.Models
{
    public interface IModel : IRenderable
    {
        List<IMesh> Meshes { get; }

        void Add(IMesh mesh);
        IModel Duplicate();
    }
}
