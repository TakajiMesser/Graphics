using OpenTK;
using SpiceEngineCore.Physics.Shapes;

namespace SpiceEngineCore.Game.Loading.Builders
{
    public interface IShapeBuilder : IComponentBuilder<IShape>
    {
        Vector3 Position { get; set; }
        //Vector3 Rotation { get; set; }
        //Vector3 Scale { get; set; }

        bool IsPhysical { get; }

        IShape ToShape();
    }
}
