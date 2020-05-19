using OpenTK;
using OpenTK.Graphics.OpenGL;
using SpiceEngine.Properties;
using SpiceEngineCore.Outputs;
using SpiceEngineCore.Rendering.Batches;
using SpiceEngineCore.Rendering.Shaders;
using SpiceEngineCore.UserInterfaces;
using SweetGraphicsCore.Buffers;
using SweetGraphicsCore.Rendering.Batches;
using SweetGraphicsCore.Rendering.Processing;
using SweetGraphicsCore.Rendering.Textures;

namespace SpiceEngine.Rendering.PostProcessing
{
    public class UIRenderer : Renderer
    {
        private ShaderProgram _uiProgram;
        private ShaderProgram _uiSelectionProgram;
        private FrameBuffer _frameBuffer = new FrameBuffer();

        public Texture FinalTexture { get; protected set; }
        
        protected override void LoadPrograms()
        {
            _uiProgram = new ShaderProgram(
                //new Shader(ShaderType.VertexShader, Resources.ui_vert),
                //new Shader(ShaderType.FragmentShader, Resources.ui_frag)
                new Shader(ShaderType.VertexShader, Resources.uiquad_vert),
                new Shader(ShaderType.GeometryShader, Resources.uiquad_geom),
                new Shader(ShaderType.FragmentShader, Resources.uiquad_frag)
            );

            _uiSelectionProgram = new ShaderProgram(
                //new Shader(ShaderType.VertexShader, Resources.ui_vert),
                //new Shader(ShaderType.FragmentShader, Resources.ui_frag)
                new Shader(ShaderType.VertexShader, Resources.uiquad_selection_vert),
                new Shader(ShaderType.GeometryShader, Resources.uiquad_selection_geom),
                new Shader(ShaderType.FragmentShader, Resources.uiquad_selection_frag)
            );
        }

        protected override void LoadTextures(Resolution resolution)
        {
            FinalTexture = new Texture(resolution.Width, resolution.Height, 0)
            {
                Target = TextureTarget.Texture2D,
                EnableMipMap = false,
                EnableAnisotropy = false,
                PixelInternalFormat = PixelInternalFormat.Rgba16f,
                PixelFormat = PixelFormat.Rgba,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            FinalTexture.Bind();
            FinalTexture.ReserveMemory();
        }

        protected override void LoadBuffers()
        {
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment0, FinalTexture);
            _frameBuffer.Bind(FramebufferTarget.Framebuffer);
            _frameBuffer.AttachAttachments();
            _frameBuffer.Unbind(FramebufferTarget.Framebuffer);
        }

        public override void ResizeTextures(Resolution resolution)
        {
            FinalTexture.Resize(resolution.Width, resolution.Height, 0);
            FinalTexture.Bind();
            FinalTexture.ReserveMemory();
        }

        /*public void Render(IBatcher batcher)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            //GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.DepthTest);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);

            // TODO - Contain all rendering logic in batcher as well in view batches
            batcher.CreateBatchAction()
                .SetShader(_uiProgram)
                .SetUniform("halfResolution", new Vector2(FinalTexture.Width / 2, FinalTexture.Height / 2))
                .SetRenderType(RenderTypes.OpaqueView)
                .Render()
                .Execute();

            //uiManager.TestDraw();

            GL.Disable(EnableCap.Blend);
            //GL.Disable(EnableCap.DepthTest);
        }

        public void Render(IUIProvider uiProvider)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            //GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.DepthTest);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);

            _uiProgram.Use();
            _uiProgram.SetUniform("halfResolution", new Vector2(FinalTexture.Width / 2, FinalTexture.Height / 2));
            uiProvider.Draw();
            //uiManager.TestDraw();

            GL.Disable(EnableCap.Blend);
        }*/

        public void RenderSelections(IBatcher batcher, IUIProvider uiProvider)
        {
            //GL.Disable(EnableCap.DepthTest);
            //GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);

            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //GL.Viewport(0, 0, FinalTexture.Width, FinalTexture.Height);
            //GL.Disable(EnableCap.DepthTest);

            // TODO - Contain all rendering logic in batcher as well in view batches
            batcher.CreateBatchAction()
                .SetShader(_uiSelectionProgram)
                .SetUniform("resolution", new Vector2(FinalTexture.Width, FinalTexture.Height))
                .SetUniform("halfResolution", new Vector2(FinalTexture.Width / 2, FinalTexture.Height / 2))
                .SetRenderType(RenderTypes.OpaqueView)
                .SetEntityIDOrder(uiProvider.GetDrawOrder())
                .Render()
                .Execute();
        }

        public void Render(IBatcher batcher, IUIProvider uiProvider)
        {
            // Clear the depth buffer, since the UI should be rendered above everything else
            //GL.Clear(ClearBufferMask.DepthBufferBit);
            
            // Enable blending for transparent rendering
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.Disable(EnableCap.DepthTest);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);

            // TODO - Contain all rendering logic in batcher as well in view batches
            batcher.CreateBatchAction()
                .SetShader(_uiProgram)
                .SetUniform("resolution", new Vector2(FinalTexture.Width, FinalTexture.Height))
                .SetUniform("halfResolution", new Vector2(FinalTexture.Width / 2, FinalTexture.Height / 2))
                .SetRenderType(RenderTypes.OpaqueView)
                .SetEntityIDOrder(uiProvider.GetDrawOrder())
                .Render()
                .Execute();

            //_uiProgram.Use();
            //_uiProgram.SetUniform("halfResolution", new Vector2(FinalTexture.Width / 2, FinalTexture.Height / 2));
            //uiProvider.Draw();

            GL.Disable(EnableCap.Blend);
        }
    }
}
