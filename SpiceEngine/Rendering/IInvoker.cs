using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceEngine.Rendering
{
    public interface IInvoker
    {
        void Run(Action action);
        Task RunAsync(Action action);
    }
}
