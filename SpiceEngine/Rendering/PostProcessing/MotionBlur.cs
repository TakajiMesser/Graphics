using OpenTK;
using OpenTK.Graphics.OpenGL;
using SpiceEngine.Outputs;
using SpiceEngine.Properties;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;

namespace SpiceEngine.Rendering.PostProcessing
{
    public class MotionBlur : PostProcess
    {
        public const string NAME = "MotionBlur";

        private ShaderProgram _dilateProgram;
        private ShaderProgram _blurProgram;

        private Texture _velocityTextureA;
        private Texture _velocityTextureB;

        public MotionBlur(Resolution resolution) : base(NAME, resolution) { }

        protected override void LoadProgram()
        {
            var dilateShader = new Shader(ShaderType.ComputeShader, Resources.dilate_frag);
            _dilateProgram = new ShaderProgram(dilateShader);

            var blurShader = new Shader(ShaderType.FragmentShader, Resources.blur_frag);
            _blurProgram = new ShaderProgram(blurShader);
        }

        protected override void LoadBuffers()
        {
            _velocityTextureA = new Texture((int)(Resolution.Width * 0.5f), (int)(Resolution.Height * 0.5f), 0)
            {
                Target = TextureTarget.Texture2D,
                EnableMipMap = false,
                EnableAnisotropy = false,
                PixelInternalFormat = PixelInternalFormat.Rg16f,
                PixelFormat = PixelFormat.Rg,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            _velocityTextureA.Bind();
            _velocityTextureA.ReserveMemory();

            _velocityTextureB = new Texture(_velocityTextureA.Width, _velocityTextureA.Height, 0)
            {
                Target = TextureTarget.Texture2D,
                EnableMipMap = false,
                EnableAnisotropy = false,
                PixelInternalFormat = PixelInternalFormat.Rg16f,
                PixelFormat = PixelFormat.Rg,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            _velocityTextureB.Bind();
            _velocityTextureB.ReserveMemory();

            FinalTexture = new Texture(Resolution.Width, Resolution.Height, 0)
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

            _frameBuffer.Add(FramebufferAttachment.ColorAttachment0, FinalTexture);
            _frameBuffer.Bind(FramebufferTarget.Framebuffer);
            _frameBuffer.AttachAttachments();
            _frameBuffer.Unbind(FramebufferTarget.Framebuffer);
        }

        public void Render(Texture velocity, Texture depth, Texture scene, float fps)
        {
            DilateVelocity(velocity);
            ApplyMotionBlur(depth, scene, fps);
        }

        private void DilateVelocity(Texture velocity)
        {
            _dilateProgram.Use();

            int blurLocation = _dilateProgram.GetUniformLocation("blur_amount");
            GL.Uniform1(blurLocation, 150);

            int sizeLocation = _dilateProgram.GetUniformLocation("texture_size");
            GL.Uniform2(sizeLocation, new Vector2(_velocityTextureA.Width, _velocityTextureA.Height));

            int directionLocation = _dilateProgram.GetUniformLocation("direction_selector");

            // Horizontal direction
            GL.Uniform1(directionLocation, 0);

            _dilateProgram.BindTexture(velocity, "source", 0);
            _dilateProgram.BindImageTexture(_velocityTextureB, "destination", 1);

            GL.DispatchCompute(_velocityTextureA.Height, 2, 1);
            GL.MemoryBarrier(MemoryBarrierFlags.ShaderImageAccessBarrierBit);

            // Vertical direction
            GL.Uniform1(directionLocation, 1);

            _dilateProgram.BindTexture(_velocityTextureB, "source", 0);
            _dilateProgram.BindImageTexture(_velocityTextureA, "destination", 1);

            GL.DispatchCompute(_velocityTextureA.Width, 2, 1);
            GL.MemoryBarrier(MemoryBarrierFlags.ShaderImageAccessBarrierBit);
        }

        private void ApplyMotionBlur(Texture depth, Texture scene, float fps)
        {
            _blurProgram.Use();

            int fpsLocation = _blurProgram.GetUniformLocation("fps_scaler");
            GL.Uniform1(fpsLocation, fps);

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
