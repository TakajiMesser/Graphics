using SweetGraphicsCore.Rendering.Textures;
using System;

namespace SauceEditorCore.Models.Components
{
    public class TextureComponent : Component
    {
        public TextureComponent(string filePath) : base(filePath) { }

        public TexturePaths TexturePaths { get; set; }

        public override void Save() => throw new NotImplementedException();
        public override void Load() => throw new NotImplementedException();

        public static bool IsValidExtension(string extension) => extension == ".png" || extension == ".jpg";
    }
}
