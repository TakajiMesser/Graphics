﻿using OpenTK;
using SpiceEngine.Entities;

namespace SauceEditor.Models.Entities
{
    public class ShapeEntity : IEntity
    {
        public int ID { get; set; }
        public Vector3 Position { get; set; }
    }
}