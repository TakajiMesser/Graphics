using SpiceEngine.Rendering.Materials;
using System;
using System.Collections.Generic;
using System.Text;

namespace SauceEditorCore.Models.Components
{
    public class MaterialComponent : Component
    {
        public MaterialComponent(string filePath) : base(filePath) { }

        public Material Material { get; set; }

        public override void Save() => throw new NotImplementedException();
        public override void Load() => throw new NotImplementedException();
    }
}
