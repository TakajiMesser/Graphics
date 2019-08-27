﻿using OpenTK;
using SpiceEngine.Maps.Builders;

namespace SpiceEngine.Maps
{
    public interface IMapEntity3D : IEntityBuilder
    {
        //Vector3 Position { get; set; }
        Vector3 Rotation { get; set; }
        Vector3 Scale { get; set; }
    }
}
