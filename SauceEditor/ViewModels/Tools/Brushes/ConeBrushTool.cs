﻿using SweetGraphicsCore.Rendering.Models;

namespace SauceEditor.ViewModels.Tools.Brushes
{
    public class ConeBrushTool : BrushTool
    {
        public ConeBrushTool() : base("Cone") { }

        public float Radius { get; set; } = 10.0f;
        public float Height { get; set; } = 10.0f;
        public int NumberOfSides{ get; set; } = 10;

        public override ModelMesh MeshShape => ModelMesh.Cone(Radius, Height, NumberOfSides);
    }
}
