using OpenTK;
using SpiceEngineCore.Physics.Shapes;

namespace SpiceEngineCore.Game.Loading.Builders
{
    public interface IShapeBuilder : IBuilder
    {
        Vector3 Position { get; set; }
        //Vector3 Rotation { get; set; }
        //Vector3 Scale { get; set; }

        bool IsPhysical { get; }

        Shape3D ToShape();
    }
}
