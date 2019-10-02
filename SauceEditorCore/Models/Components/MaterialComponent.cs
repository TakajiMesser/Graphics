using SpiceEngineCore.Rendering.Materials;
using System;

namespace SauceEditorCore.Models.Components
{
    public class MaterialComponent : Component
    {
        public MaterialComponent(string filePath) : base(filePath) { }

        public Material Material { get; set; }

        public override void Save() => throw new NotImplementedException();
        public override void Load() => throw new NotImplementedException();

        public static bool IsValidExtension(string extension) => extension == ".mtl";
    }
}
