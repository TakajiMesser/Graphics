using SpiceEngineCore.Entities;
using SpiceEngineCore.Rendering.Materials;
using SpiceEngineCore.Rendering.Shaders;
using SpiceEngineCore.Rendering.Textures;
using System;

namespace SpiceEngine.Entities
{
    public abstract class TexturedEntity : Entity, ITextureBinder
    {
        public abstract Material Material { get; }
        public abstract TextureMapping? TextureMapping { get; }

        public event EventHandler<TextureTransformEventArgs> TextureTransformed;

        public abstract void AddMaterial(Material material);
        public abstract void AddTextureMapping(TextureMapping? textureMapping);

        public void BindTextures(ShaderProgram program, ITextureProvider textureProvider)
        {
            if (TextureMapping.HasValue)
            {
                program.BindTextures(textureProvider, TextureMapping.Value);
            }
            else
            {
                program.UnbindTextures();
            }
        }

        public override void SetUniforms(ShaderProgram program) => Material.SetUniforms(program);

        public override bool CompareUniforms(IEntity entity) => entity is ITextureBinder textureBinder
            && Material.Equals(textureBinder.Material)
            && TextureMapping.Equals(textureBinder.TextureMapping);

        protected virtual void OnTextureTransformed(object sender, TextureTransformEventArgs e) => TextureTransformed?.Invoke(sender, e);
    }
}
