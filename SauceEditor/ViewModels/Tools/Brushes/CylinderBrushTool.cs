﻿using SweetGraphicsCore.Rendering.Models;

namespace SauceEditor.ViewModels.Tools.Brushes
{
    public class CylinderBrushTool : BrushTool
    {
        public CylinderBrushTool() : base("Cylinder") { }

        public float Radius { get; set; } = 10.0f;
        public float Height { get; set; } = 10.0f;
        public int NumberOfSides { get; set; } = 10;

        public override ModelMesh MeshShape => ModelMesh.Cylinder(Radius, Height, NumberOfSides);
    }
}
