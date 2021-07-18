using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngineCore.Rendering;
using SweetGraphicsCore.Buffers;
using SweetGraphicsCore.Helpers;
using SweetGraphics.Properties;
using SweetGraphicsCore.Rendering.Textures;
using SweetGraphicsCore.Shaders;

namespace SweetGraphicsCore.Renderers.PostProcessing
{
    public class MotionBlur : Renderer
    {
        public const string NAME = "MotionBlur";

        private ShaderProgram _dilateProgram;
        private ShaderProgram _blurProgram;

        private Texture _velocityTextureA;
        private Texture _velocityTextureB;

        private FrameBuffer _frameBuffer;

        public Texture FinalTexture { get; protected set; }

        protected override void LoadPrograms(IRenderContext renderContext)
        {
            _dilateProgram = ShaderHelper.LoadProgram(renderContext,
                new[] { ShaderType.ComputeShader },
                new[] { Resources.dilate_frag });

            _blurProgram = ShaderHelper.LoadProgram(renderContext,
                new[] { ShaderType.FragmentShader },
                new[] { Resources.blur_frag });
        }

        protected override void LoadTextures(IRenderContext renderContext, Resolution resolution)
        {
            _velocityTextureA = new Texture(renderContext, (int)(resolution.Width * 0.5f), (int)(resolution.Height * 0.5f), 0)
            {
                Target = TextureTarget.Texture2d,
                EnableMipMap = false,
                EnableAnisotropy = false,
                InternalFormat = InternalFormat.Rg16f,
                PixelFormat = PixelFormat.Rg,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            _velocityTextureA.Load();

            _velocityTextureB = new Texture(renderContext, _velocityTextureA.Width, _velocityTextureA.Height, 0)
            {
                Target = TextureTarget.Texture2d,
                EnableMipMap = false,
                EnableAnisotropy = false,
                InternalFormat = InternalFormat.Rg16f,
                PixelFormat = PixelFormat.Rg,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            _velocityTextureB.Load();

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

        public void Render(Texture velocity, Texture depth, Texture scene, float fps)
        {
            DilateVelocity(velocity);
            ApplyMotionBlur(depth, scene, fps);
        }

        private void DilateVelocity(Texture velocity)
        {
            _dilateProgram.Bind();

            int blurLocation = _dilateProgram.GetUniformLocation("blur_amount");
            GL.Uniform1i(blurLocation, 50/*150*/);

            int sizeLocation = _dilateProgram.GetUniformLocation("texture_size");
            GL.Uniform2i(sizeLocation, _velocityTextureA.Width, _velocityTextureA.Height);

            int directionLocation = _dilateProgram.GetUniformLocation("direction_selector");

            // Horizontal direction
            GL.Uniform1i(directionLocation, 0);

            _dilateProgram.BindTexture(velocity, "source", 0);
            _dilateProgram.BindImageTexture(_velocityTextureB, "destination", 1);

            GL.DispatchCompute(_velocityTextureA.Height, 2, 1);
            GL.MemoryBarrier(MemoryBarrierMask.ShaderImageAccessBarrierBit);

            // Vertical direction
            GL.Uniform1i(directionLocation, 1);

            _dilateProgram.BindTexture(_velocityTextureB, "source", 0);
            _dilateProgram.BindImageTexture(_velocityTextureA, "destination", 1);

            GL.DispatchCompute(_velocityTextureA.Width, 2, 1);
            GL.MemoryBarrier(MemoryBarrierMask.ShaderImageAccessBarrierBit);
        }

        private void ApplyMotionBlur(Texture depth, Texture scene, float fps)
        {
            _blurProgram.Bind();

            int fpsLocation = _blurProgram.GetUniformLocation("fps_scaler");
            GL.Uniform1f(fpsLocation, fps);

            _blurProgram.BindTexture(_velocityTextureA, "velocity", 1);
            _blurProgram.BindTexture(depth, "depth", 2);

            // Pass 1
            _frameBuffer.BindAndDraw();

            _blurProgram.BindTexture(scene, "source", 0);

            // quad.Render();

            // Pass 2
            // sceneFrames.Bind(FramebufferTarget.DrawFramebuffer);
            // GL.DrawBuffer(DrawBufferMode.ColorAttachment0);

            _blurProgram.BindTexture(FinalTexture, "source", 0);

            // quad.Render();
        }
    }
}
