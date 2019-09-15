using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Physics.Shapes;
using SpiceEngine.Rendering;
using System;

namespace SpiceEngine.Entities.Builders
{
    public interface IShapeBuilder
    {
        Vector3 Position { get; set; }
        //Vector3 Rotation { get; set; }
        //Vector3 Scale { get; set; }

        bool IsPhysical { get; }

        Shape3D ToShape();
        Type GetEntityType();
    }
}
