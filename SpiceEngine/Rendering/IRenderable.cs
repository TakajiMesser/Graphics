using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceEngine.Rendering
{
    public interface IRenderable
    {
        bool IsAnimated { get; }
        bool IsTransparent { get; }

        event EventHandler<AlphaEventArgs> AlphaChanged;
    }
}
