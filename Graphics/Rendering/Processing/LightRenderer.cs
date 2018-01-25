using Graphics.GameObjects;
using Graphics.Helpers;
using Graphics.Lighting;
using Graphics.Meshes;
using Graphics.Outputs;
using Graphics.Rendering.Buffers;
using Graphics.Rendering.Shaders;
using Graphics.Rendering.Textures;
using Graphics.Rendering.Vertices;
using Graphics.Utilities;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Rendering.Processing
{
    public class LightRenderer
    {
        public Resolution Resolution { get; private set; }

        public Texture FinalTexture { get; protected set; }
        public Texture DepthTexture { get; protected set; }

        //public Texture DepthStencilTexture { get; protected set; }
        //public Texture DiffuseTexture { get; protected set; }
        //public Texture SpecularTexture { get; protected set; }

        //internal ShaderProgram _stencilProgram;
        internal ShaderProgram _pointLightProgram;
        //internal ShaderProgram _spotLightProgram;

        private SimpleMesh _pointLightMesh;
        private int _vertexArrayHandle;
        private VertexBuffer<Vector3> _vertexBuffer = new VertexBuffer<Vector3>();

        private FrameBuffer _frameBuffer = new FrameBuffer();

        public LightRenderer(Resolution resolution)
        {
            Resolution = resolution;
        }

        public void Load()
        {
            LoadPrograms();
            LoadBuffers();
            LoadQuad(_pointLightProgram);
        }

        protected void LoadQuad(ShaderProgram program)
        {
            _vertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayHandle);
            _vertexBuffer.AddVertices(new[]
            {
                new Vector3(1.0f, 1.0f, 0.0f),
                new Vector3(-1.0f, 1.0f, 0.0f),
                new Vector3(-1.0f, -1.0f, 0.0f),
                new Vector3(1.0f, -1.0f, 0.0f)
            });
            _vertexBuffer.Bind();
            _vertexBuffer.Buffer();

            var attribute = new VertexAttribute("vPosition", 3, VertexAttribPointerType.Float, UnitConversions.SizeOf<Vector3>(), 0);
            attribute.Set(program.GetAttributeLocation("vPosition"));

            GL.BindVertexArray(0);
            _vertexBuffer.Unbind();
        }

        protected void RenderQuad()
        {
            GL.BindVertexArray(_vertexArrayHandle);
            _vertexBuffer.Bind();

            _vertexBuffer.DrawQuads();

            GL.BindVertexArray(0);
            _vertexBuffer.Unbind();
        }

        protected void LoadPrograms()
        {
            /*_stencilProgram = new ShaderProgram(new[] {
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.LIGHT_VERTEX_SHADER_PATH))
            });*/

            _pointLightProgram = new ShaderProgram(new[] {
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.LIGHT_VERTEX_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.POINT_LIGHT_FRAGMENT_SHADER_PATH))
            });

            /*_spotLightProgram = new ShaderProgram(new[] {
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.LIGHT_VERTEX_SHADER_PATH)),
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.SPOT_LIGHT_FRAGMENT_SHADER_PATH))
            });*/
        }

        public void ResizeTextures()
        {
            FinalTexture.Resize(Resolution.Width, Resolution.Height, 0);
            FinalTexture.Bind();
            FinalTexture.ReserveMemory();

            DepthTexture.Resize(Resolution.Width, Resolution.Height, 0);
            DepthTexture.Bind();
            DepthTexture.ReserveMemory();

            /*DepthStencilTexture.Resize(Resolution.Width, Resolution.Height, 0);
            DepthStencilTexture.Bind();
            DepthStencilTexture.ReserveMemory();

            DiffuseTexture.Resize(Resolution.Width, Resolution.Height, 0);
            DiffuseTexture.Bind();
            DiffuseTexture.ReserveMemory();

            SpecularTexture.Resize(Resolution.Width, Resolution.Height, 0);
            SpecularTexture.Bind();
            SpecularTexture.ReserveMemory();*/
        }

        protected void LoadBuffers()
        {
            /*DepthStencilTexture = new Texture(Resolution.Width, Resolution.Height, 0)
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
            DepthStencilTexture.Bind();
            DepthStencilTexture.ReserveMemory();

            DiffuseTexture = new Texture(Resolution.Width, Resolution.Height, 0)
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
            DiffuseTexture.Bind();
            DiffuseTexture.ReserveMemory();

            SpecularTexture = new Texture(Resolution.Width, Resolution.Height, 0)
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
            SpecularTexture.Bind();
            SpecularTexture.ReserveMemory();*/

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

            DepthTexture = new Texture(Resolution.Width, Resolution.Height, 0)
            {
                Target = TextureTarget.Texture2D,
                EnableMipMap = false,
                EnableAnisotropy = false,
                PixelInternalFormat = PixelInternalFormat.DepthComponent32f,
                PixelFormat = PixelFormat.DepthComponent,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            DepthTexture.Bind();
            DepthTexture.ReserveMemory();

            _frameBuffer.Clear();
            //_frameBuffer.Add(FramebufferAttachment.ColorAttachment0, DiffuseTexture);
            //_frameBuffer.Add(FramebufferAttachment.ColorAttachment1, SpecularTexture);
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment6, FinalTexture);
            _frameBuffer.Add(FramebufferAttachment.DepthAttachment, DepthTexture);

            _frameBuffer.Bind();
            _frameBuffer.AttachAttachments();
            _frameBuffer.Unbind();

            _pointLightMesh = new SimpleMesh(
                new List<Vector3>
                {
                    new Vector3(0, -0.525731f, 0.850651f),
                    new Vector3(0.850651f, 0, 0.525731f),
                    new Vector3(0.850651f, 0, -0.525731f),
                    new Vector3(-0.850651f, 0, -0.525731f),
                    new Vector3(-0.850651f, 0,  0.525731f),
                    new Vector3(-0.525731f, 0.850651f, 0),
                    new Vector3(0.525731f, 0.850651f, 0),
                    new Vector3(0.525731f, -0.850651f, 0),
                    new Vector3(-0.525731f, -0.850651f, 0),
                    new Vector3(0, -0.525731f, -0.850651f),
                    new Vector3(0, 0.525731f, -0.850651f),
                    new Vector3(0, 0.525731f, 0.850651f)
                },
                new List<int>
                {
                    1, 2, 6,
                    1, 7, 2,
                    3, 4, 5,
                    4, 3, 8,
                    6, 5, 11,
                    5, 6, 10,
                    9, 10, 2,
                    10, 9, 3,
                    7, 8, 9,
                    8, 7, 0,
                    11, 0, 1,
                    0, 11, 4,
                    6, 2, 10,
                    1, 6, 11,
                    3, 5, 10,
                    5, 4, 11,
                    2, 7, 9,
                    7, 1, 0,
                    3, 9, 8,
                    4, 8, 0
                },
                _pointLightProgram
            );
        }

        private void StencilPass()
        {

        }

        private void PointLightPass()
        {

        }

        private void SpotLightPass()
        {

        }

        public void Render(Texture color, Texture normalDepth, Texture ambient, Texture diffuseMaterial, Texture specular, FrameBuffer gBuffer, Camera camera, IEnumerable<PointLight> pointLights)
        {
            //GL.DepthMask(false);
            //GL.Disable(EnableCap.DepthTest);

            // Stencil pass

            // Light pass
            _pointLightProgram.Use();
            //gBuffer.Draw();
            //GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, gBuffer._handle);
            //GL.DrawBuffer(DrawBufferMode.ColorAttachment6);
            //GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
            //_frameBuffer.Draw();

            GL.Enable(EnableCap.Blend);
            GL.BlendEquation(BlendEquationMode.FuncAdd);
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.One);

            //GL.ClearColor(Color4.Black);
            //GL.Clear(ClearBufferMask.ColorBufferBit);
            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            // Can I just ignore the GBuffer if I bind the textures directly in here? Or is there some efficiency gain in passing the GBuffer through?
            _pointLightProgram.BindTexture(color, "colorMap", 0);
            _pointLightProgram.BindTexture(normalDepth, "normalDepth", 1);
            _pointLightProgram.BindTexture(ambient, "ambientMap", 2);
            _pointLightProgram.BindTexture(diffuseMaterial, "diffuseMaterial", 3);
            _pointLightProgram.BindTexture(specular, "specularMap", 4);

            //GL.Clear(ClearBufferMask.ColorBufferBit);

            camera.Draw(_pointLightProgram);
            _pointLightProgram.SetUniform("cameraPosition", camera.Position);

            foreach (var light in pointLights)
            {
                // Draw the light (bind the uniforms)
                light.Draw(_pointLightProgram);
                //_pointLightMesh.Draw();
                RenderQuad();
            }

            GL.Disable(EnableCap.Blend);
        }

        private IEnumerable<GameObject> PerformFrustumCulling(IEnumerable<GameObject> gameObjects)
        {
            // Don't render meshes that are not in the camera's view

            // Using the position of the gameObject, determine if we should render the mesh
            // We will also need a bounding sphere or bounding box from the mesh to determine this
            foreach (var gameObject in gameObjects)
            {
                Vector3 position = gameObject.Position;
            }

            return gameObjects;
        }

        private IEnumerable<GameObject> PerformOcclusionCulling(IEnumerable<GameObject> gameObjects)
        {
            // Don't render meshes that are obscured by closer meshes
            foreach (var gameObject in gameObjects)
            {
                Vector3 position = gameObject.Position;
            }

            return gameObjects;
        }
    }
}
