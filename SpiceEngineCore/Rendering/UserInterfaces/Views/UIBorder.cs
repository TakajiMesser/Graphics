using OpenTK.Graphics;
using SpiceEngineCore.Rendering.Buffers;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.UserInterfaces.Attributes;
using SpiceEngineCore.Rendering.UserInterfaces.Layers;
using SpiceEngineCore.Rendering.Vertices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngineCore.Rendering.UserInterfaces.Views
{
    public class UIBorder
    {
        public float Thickness { get; set; } = 5.0f;
        public Color4 Color { get; set; }

        public float RoundedRadius { get; set; } = 0.0f;
    }
}
