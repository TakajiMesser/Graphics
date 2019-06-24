using SpiceEngine.Rendering.Textures;
using System;
using System.Collections.Generic;
using System.Text;

namespace SauceEditorCore.Models.Components
{
    public class TextureComponent : Component
    {
        public TextureComponent(string filePath) : base(filePath) { }

        public TexturePaths TexturePaths { get; set; }

        public override void Save() => throw new NotImplementedException();
        public override void Load() => throw new NotImplementedException();
    }
}
