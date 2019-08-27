using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Physics.Shapes;
using SpiceEngine.Rendering;

namespace SpiceEngine.Maps.Builders
{
    public interface IRenderableBuilder
    {
        Vector3 Position { get; set; }

        IRenderable ToRenderable();
    }
}
