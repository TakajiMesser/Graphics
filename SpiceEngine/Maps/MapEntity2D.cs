﻿using OpenTK;
using SpiceEngine.Entities;

namespace SpiceEngine.Maps
{
    public abstract class MapEntity2D<T> where T : IEntity
    {
        public Vector2 Position { get; set; } = Vector2.Zero;
        public Vector2 Rotation { get; set; } = Vector2.Zero;
        public Vector2 Scale { get; set; } = Vector2.One;

        public abstract T ToEntity();
    }
}
