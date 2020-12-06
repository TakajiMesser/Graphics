using SpiceEngineCore.Rendering.Materials;
using SpiceEngineCore.Rendering.Textures;
using System;
using System.Collections.Generic;

namespace SpiceEngineCore.Entities
{
    public abstract class TexturedEntity : Entity, ITextureBinder
    {
        public abstract IEnumerable<Material> Materials { get; }
        public abstract IEnumerable<TextureMapping?> TextureMappings { get; }

        public abstract Material CurrentMaterial { get; }
        public abstract TextureMapping? CurrentTextureMapping { get; }

        public event EventHandler<TextureTransformEventArgs> TextureTransformed;

        public abstract void AddMaterial(Material material);
        public abstract void AddTextureMapping(TextureMapping? textureMapping);

        /*public void BindTextures(ShaderProgram program, ITextureProvider textureProvider)
        {
            if (CurrentTextureMapping.HasValue)
            {
                program.BindTextures(textureProvider, CurrentTextureMapping.Value);
            }
            else
            {
                program.UnbindTextures();
            }
        }*/

        /*public override void SetUniforms(ShaderProgram program) => CurrentMaterial.SetUniforms(program);

        public override bool CompareUniforms(IEntity entity) => entity is ITextureBinder textureBinder
            && CurrentMaterial.Equals(textureBinder.CurrentMaterial)
            && TextureMappings.Equals(textureBinder.TextureMappings);*/

        protected virtual void OnTextureTransformed(object sender, TextureTransformEventArgs e) => TextureTransformed?.Invoke(sender, e);
    }
}
