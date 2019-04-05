﻿using OpenTK;

namespace SpiceEngine.Maps
{
    public interface IMapEntity3D
    {
        Vector3 Position { get; set; }
        Vector3 Rotation { get; set; }
        Vector3 Scale { get; set; }
    }
}