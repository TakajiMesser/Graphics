using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.IO;
using TakoEngine.Entities;
using TakoEngine.Entities.Cameras;
using TakoEngine.Entities.Lights;
using TakoEngine.Entities.Models;
using TakoEngine.Helpers;
using TakoEngine.Outputs;
using TakoEngine.Rendering.Buffers;
using TakoEngine.Rendering.Matrices;
using TakoEngine.Rendering.Meshes;
using TakoEngine.Rendering.Shaders;
using TakoEngine.Rendering.Textures;
using TakoEngine.Rendering.Vertices;

namespace TakoEngine.Rendering.Processing
{
    public class WireframeRenderer : Renderer
    {
        public float LineThickness { get; set; } = 0.01f;
        public float SelectedLineThickness { get; set; } = 0.02f;
        public float GridLength { get; set; } = 100.0f;
        public Quaternion GridRotation { get; set; } = Quaternion.Identity;

        public Texture FinalTexture { get; protected set; }
        public Texture DepthStencilTexture { get; protected set; }

        private ShaderProgram _wireframeProgram;
        private ShaderProgram _jointWireframeProgram;
        private ShaderProgram _gridProgram;

        private FrameBuffer _frameBuffer = new FrameBuffer();
        private SimpleMesh _gridSquare;

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

            _gridProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.GRID_VERTEX_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.GRID_FRAGMENT_SHADER_PATH))
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
            _frameBuffer.Clear();
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment0, FinalTexture);
            _frameBuffer.Add(FramebufferAttachment.DepthStencilAttachment, DepthStencilTexture);

            _frameBuffer.Bind(FramebufferTarget.Framebuffer);
            _frameBuffer.AttachAttachments();
            _frameBuffer.Unbind(FramebufferTarget.Framebuffer);

            _gridSquare = SimpleMesh.LoadFromFile(FilePathHelper.SQUARE_MESH_PATH, _gridProgram);
        }

        public void BindForWriting()
        {
            _frameBuffer.BindAndDraw(DrawBuffersEnum.ColorAttachment0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.DepthMask(false);
            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.CullFace);
        }

        public void WireframePass(Camera camera, IEnumerable<Brush> brushes, IEnumerable<Actor> actors)
        {
            _wireframeProgram.Use();

            camera.SetUniforms(_wireframeProgram);
            _wireframeProgram.SetUniform("lineThickness", LineThickness);
            _wireframeProgram.SetUniform("lineColor", Vector4.One);

            foreach (var brush in brushes)
            {
                brush.Draw(_wireframeProgram);
            }

            foreach (var actor in actors)
            {
                actor.Draw(_wireframeProgram);
            }
        }

        public void JointWireframePass(Camera camera, IEnumerable<Actor> actors)
        {
            _jointWireframeProgram.Use();

            camera.SetUniforms(_jointWireframeProgram);
            _jointWireframeProgram.SetUniform("lineThickness", LineThickness);
            _jointWireframeProgram.SetUniform("lineColor", Vector4.One);

            foreach (var actor in actors)
            {
                actor.Draw(_jointWireframeProgram);
            }
        }

        public void RenderGridLines(Camera camera)
        {
            _gridProgram.Use();

            camera.SetUniforms(_gridProgram);

            var model = Matrix4.Identity * Matrix4.CreateFromQuaternion(GridRotation) * Matrix4.CreateScale(GridLength);
            _gridProgram.SetUniform("modelMatrix", model);

            _gridProgram.SetUniform("thickness", 0.02f);
            _gridProgram.SetUniform("length", GridLength);

            _gridSquare.Draw();
        }

        public void SelectionPass(Camera camera, IEntity entity)
        {
            var program = (entity is Actor actor && actor.Model is AnimatedModel) ? _jointWireframeProgram : _wireframeProgram;
            program.Use();

            camera.SetUniforms(program);
            program.SetUniform("lineThickness", SelectedLineThickness);
            program.SetUniform("lineColor", new Vector4(0.7f, 0.7f, 0.1f, 1.0f));

            switch (entity)
            {
                case Brush b:
                    b.Draw(_wireframeProgram);
                    break;
                case Actor a:
                    a.Draw(_wireframeProgram);
                    break;
                case Light l:
                    break;
            }
        }

        public void SelectionPass(Camera camera, Light light, SimpleMesh mesh)
        {
            _wireframeProgram.Use();

            camera.SetUniforms(_wireframeProgram);
            _wireframeProgram.SetUniform("lineThickness", SelectedLineThickness);
            _wireframeProgram.SetUniform("lineColor", new Vector4(0.8f, 0.8f, 0.1f, 1.0f));

            switch (light)
            {
                case PointLight p:
                    _wireframeProgram.SetUniform("modelMatrix", Matrix4.Identity * Matrix4.CreateScale(p.Radius) * Matrix4.CreateTranslation(p.Position));
                    break;
                case SpotLight s:
                    _wireframeProgram.SetUniform("modelMatrix", s.Model);
                    break;
            }

            mesh.Draw();
        }

        private IEnumerable<Actor> PerformFrustumCulling(IEnumerable<Actor> actors)
        {
            // Don't render meshes that are not in the camera's view

            // Using the position of the actor, determine if we should render the mesh
            // We will also need a bounding sphere or bounding box from the mesh to determine this
            foreach (var actor in actors)
            {
                Vector3 position = actor.Model.Position;
            }

            return actors;
        }

        private IEnumerable<Actor> PerformOcclusionCulling(IEnumerable<Actor> actors)
        {
            // Don't render meshes that are obscured by closer meshes
            foreach (var actor in actors)
            {
                Vector3 position = actor.Model.Position;
            }

            return actors;
        }
    }
}
