using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngine.Properties;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Batches;
using StarchUICore;
using SweetGraphicsCore.Buffers;
using SweetGraphicsCore.Helpers;
using SweetGraphicsCore.Renderers;
using SweetGraphicsCore.Rendering.Batches;
using SweetGraphicsCore.Rendering.Textures;
using SweetGraphicsCore.Shaders;

namespace SpiceEngine.Rendering.PostProcessing
{
    public class UIRenderer : Renderer
    {
        private ShaderProgram _uiProgram;
        private ShaderProgram _uiSelectionProgram;
        private FrameBuffer _frameBuffer;

        public Texture FinalTexture { get; protected set; }
        
        protected override void LoadPrograms(IRenderContext renderContext)
        {
            _uiProgram = ShaderHelper.LoadProgram(renderContext,
                new[] { ShaderType.VertexShader, ShaderType.GeometryShader, ShaderType.FragmentShader },
                new[] { Resources.uiquad_vert, Resources.uiquad_geom, Resources.uiquad_frag });

            _uiSelectionProgram = ShaderHelper.LoadProgram(renderContext,
                new[] { ShaderType.VertexShader, ShaderType.GeometryShader, ShaderType.FragmentShader },
                new[] { Resources.uiquad_selection_vert, Resources.uiquad_selection_geom, Resources.uiquad_selection_frag });
        }

        protected override void LoadTextures(IRenderContext renderContext, Resolution resolution)
        {
            FinalTexture = new Texture(renderContext, resolution.Width, resolution.Height, 0)
            {
                Target = TextureTarget.Texture2d,
                EnableMipMap = false,
                EnableAnisotropy = false,
                InternalFormat = InternalFormat.Rgba16f,
                PixelFormat = PixelFormat.Rgba,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            FinalTexture.Load();
        }

        protected override void LoadBuffers(IRenderContext renderContext)
        {
            _frameBuffer = new FrameBuffer(renderContext);
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment0, FinalTexture);
            _frameBuffer.Load();
        }

        protected override void Resize(Resolution resolution) => FinalTexture.Resize(resolution.Width, resolution.Height, 0);

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
