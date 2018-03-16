using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.IO;
using TakoEngine.Entities;
using TakoEngine.Entities.Cameras;
using TakoEngine.Helpers;
using TakoEngine.Outputs;
using TakoEngine.Rendering.Buffers;
using TakoEngine.Rendering.Shaders;
using TakoEngine.Rendering.Textures;

namespace TakoEngine.Rendering.Processing
{
    public class WireframeRenderer : Renderer
    {
        public float LineThickness { get; set; } = 0.01f;

        public Texture FinalTexture { get; protected set; }
        public Texture DepthStencilTexture { get; protected set; }

        public FrameBuffer GBuffer { get; private set; } = new FrameBuffer();

        internal ShaderProgram _wireframeProgram;
        internal ShaderProgram _jointWireframeProgram;

        protected override void LoadPrograms()
        {
            _wireframeProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.WIREFRAME_VERTEX_SHADER_PATH)),
                new Shader(ShaderType.GeometryShader, File.ReadAllText(FilePathHelper.WIREFRAME_GEOMETRY_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.WIREFRAME_FRAGMENT_SHADER_PATH))
            );

            _jointWireframeProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.WIREFRAME_SKINNING_VERTEX_SHADER_PATH)),
                new Shader(ShaderType.GeometryShader, File.ReadAllText(FilePathHelper.WIREFRAME_GEOMETRY_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.WIREFRAME_FRAGMENT_SHADER_PATH))
            );
        }

        public override void ResizeTextures(Resolution resolution)
        {
            FinalTexture.Resize(resolution.Width, resolution.Height, 0);
            FinalTexture.Bind();
            FinalTexture.ReserveMemory();

            DepthStencilTexture.Resize(resolution.Width, resolution.Height, 0);
            DepthStencilTexture.Bind();
            DepthStencilTexture.ReserveMemory();
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

            DepthStencilTexture = new Texture(resolution.Width, resolution.Height, 0)
            {
                Target = TextureTarget.Texture2D,
                EnableMipMap = false,
                EnableAnisotropy = false,
                PixelInternalFormat = PixelInternalFormat.Depth32fStencil8,
                PixelFormat = PixelFormat.DepthComponent,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            DepthStencilTexture.Bind();
            DepthStencilTexture.ReserveMemory();
        }

        protected override void LoadBuffers()
        {
            GBuffer.Clear();
            GBuffer.Add(FramebufferAttachment.ColorAttachment0, FinalTexture);
            GBuffer.Add(FramebufferAttachment.DepthStencilAttachment, DepthStencilTexture);

            GBuffer.Bind(FramebufferTarget.Framebuffer);
            GBuffer.AttachAttachments();
            GBuffer.Unbind(FramebufferTarget.Framebuffer);
        }

        public void WireframePass(Resolution resolution, Camera camera, IEnumerable<Brush> brushes, IEnumerable<Actor> gameObjects)
        {
            // Clear final texture from last frame
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, GBuffer._handle);
            GL.DrawBuffer(DrawBufferMode.ColorAttachment6);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            _wireframeProgram.Use();
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, GBuffer._handle);
            GL.DrawBuffer(DrawBufferMode.ColorAttachment0);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.DepthMask(false);
            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.CullFace);

            //GL.PolygonMode(MaterialFace.FrontAndBack, WireFrame ? PolygonMode.Line : PolygonMode.Fill);

            camera.Draw(_wireframeProgram);
            //var viewport = Matrix4.
            //_wireframeProgram.SetUniform("viewportMatrix", 0.0f);
            _wireframeProgram.SetUniform("lineThickness", LineThickness);

            foreach (var brush in brushes)
            {
                brush.Draw(_wireframeProgram);
            }

            foreach (var actor in gameObjects)
            {
                actor.Draw(_wireframeProgram);
            }
        }

        public void JointWireframePass(Resolution resolution, Camera camera, IEnumerable<Actor> gameObjects)
        {
            _jointWireframeProgram.Use();
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, GBuffer._handle);
            GL.DrawBuffer(DrawBufferMode.ColorAttachment0);

            GL.Disable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);

            //GL.PolygonMode(MaterialFace.FrontAndBack, WireFrame ? PolygonMode.Line : PolygonMode.Fill);

            camera.Draw(_jointWireframeProgram);
            _jointWireframeProgram.SetUniform("lineThickness", LineThickness);

            foreach (var actor in gameObjects)
            {
                actor.Draw(_jointWireframeProgram);
            }

            GL.Enable(EnableCap.CullFace);
        }

        private IEnumerable<Actor> PerformFrustumCulling(IEnumerable<Actor> gameObjects)
        {
            // Don't render meshes that are not in the camera's view

            // Using the position of the actor, determine if we should render the mesh
            // We will also need a bounding sphere or bounding box from the mesh to determine this
            foreach (var actor in gameObjects)
            {
                Vector3 position = actor.Model.Position;
            }

            return gameObjects;
        }

        private IEnumerable<Actor> PerformOcclusionCulling(IEnumerable<Actor> gameObjects)
        {
            // Don't render meshes that are obscured by closer meshes
            foreach (var actor in gameObjects)
            {
                Vector3 position = actor.Model.Position;
            }

            return gameObjects;
        }
    }
}
