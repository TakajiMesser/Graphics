using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Batches;
using SpiceEngineCore.Rendering.Textures;
using SweetGraphicsCore.Renderers.Shaders;

namespace SweetGraphicsCore.Renderers
{
    public abstract class Renderer : IRenderer
    {
        protected ITextureProvider _textureProvider;

        public Renderer(ITextureProvider textureProvider) => _textureProvider = textureProvider;

        public void Load(Resolution resolution)
        {
            LoadShaders();
            LoadTextures(resolution);
            LoadBuffers();
        }

        public abstract void Resize(Resolution resolution);

        protected abstract void LoadShaders();
        protected abstract void LoadTextures(Resolution resolution);
        protected abstract void LoadBuffers();

        protected void RenderBatch(ShaderProgram program, IBatcher batcher, IBatch batch)
        {
            foreach (var uniform in batch.GetUniforms(batcher))
            {
                program.SetUniform(uniform);
            }

            foreach (var textureBinding in batch.GetTextureBindings(_textureProvider))
            {
                program.BindTextures(textureBinding);
            }

            batch.Draw();
        }
    }
}
