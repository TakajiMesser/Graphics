using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Physics.Shapes;
using SpiceEngine.Rendering;
using System;

namespace SpiceEngine.Entities.Builders
{
    public class RendererLoadEventArgs : EventArgs
    {
        public string Name { get; private set; }

        public RendererLoadEventArgs(string name) => Name = name;
    }
}
