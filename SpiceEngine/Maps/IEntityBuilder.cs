using OpenTK;

namespace SpiceEngine.Maps
{
    public interface IEntityBuilder
    {
        Vector3 Position { get; set; }
        //Vector3 Rotation { get; set; }
        //Vector3 Scale { get; set; }

        IEntity ToEntity();
        IRenderable ToRenderable();
        Shape3D ToShape();
    }
}
