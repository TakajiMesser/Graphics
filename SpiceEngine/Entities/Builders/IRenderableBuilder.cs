﻿using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Physics.Shapes;
using SpiceEngine.Rendering;
using System;

namespace SpiceEngine.Entities.Builders
{
    public interface IRenderableBuilder
    {
        Vector3 Position { get; set; }

        IRenderable ToRenderable();
    }
}
