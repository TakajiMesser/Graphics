using SpiceEngineCore.Rendering;
using SweetGraphicsCore.Rendering.Meshes;
using System.Collections.Generic;

namespace SweetGraphicsCore.Rendering.Models
{
    public interface IModel : IRenderable
    {
        List<IMesh> Meshes { get; }

        void Add(IMesh mesh);
        IModel Duplicate();
    }
}
