using OpenTK;
using SpiceEngineCore.Physics;

namespace SpiceEngineCore.Components.Builders
{
    public interface IShapeBuilder : IComponentBuilder<IShape>
    {
        Vector3 Position { get; set; }
        //Vector3 Rotation { get; set; }
        //Vector3 Scale { get; set; }

        bool IsPhysical { get; }
    }
}
