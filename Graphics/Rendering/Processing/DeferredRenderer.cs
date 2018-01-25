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
    public class DeferredRenderer
    {
        public Resolution Resolution { get; private set; }

        public Texture PositionTexture { get; protected set; }
        public Texture ColorTexture { get; protected set; }
        public Texture NormalDepthTexture { get; protected set; }
        public Texture DiffuseMaterialTexture { get; protected set; }
        public Texture SpecularTexture { get; protected set; }
        public Texture VelocityTexture { get; protected set; }
        public Texture FinalTexture { get; protected set; }
        public Texture DepthStencilTexture { get; protected set; }

        public FrameBuffer GBuffer { get; private set; } = new FrameBuffer();

        internal ShaderProgram _geometryProgram;
        internal ShaderProgram _stencilProgram;
        internal ShaderProgram _pointLightProgram;
        internal ShaderProgram _simpleProgram;

        private SimpleMesh _pointLightMesh;
        private int _vertexArrayHandle;
        private VertexBuffer<Vector3> _vertexBuffer = new VertexBuffer<Vector3>();

        public DeferredRenderer(Resolution resolution)
        {
            Resolution = resolution;
        }

        public void Load()
        {
            LoadPrograms();
            LoadBuffers();
            LoadQuad(_pointLightProgram);
        }

        protected void LoadPrograms()
        {
            _geometryProgram = new ShaderProgram(new[] {
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.GEOMETRY_VERTEX_SHADER_PATH)),
                //new Shader(ShaderType.TessControlShader, File.ReadAllText(FilePathHelper.GEOMETRY_TESS_CONTROL_SHADER_PATH)),
                //new Shader(ShaderType.TessEvaluationShader, File.ReadAllText(FilePathHelper.GEOMETRY_TESS_EVAL_SHADER_PATH)),
                //new Shader(ShaderType.GeometryShader, File.ReadAllText(FilePathHelper.GEOMETRY_GEOMETRY_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.GEOMETRY_FRAGMENT_SHADER_PATH))
            });

            _stencilProgram = new ShaderProgram(new[]
            {
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.STENCIL_VERTEX_SHADER_PATH))
            });

            _pointLightProgram = new ShaderProgram(new[] {
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.LIGHT_VERTEX_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.POINT_LIGHT_FRAGMENT_SHADER_PATH))
            });

            _simpleProgram = new ShaderProgram(new[] {
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.SIMPLE_VERTEX_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.SIMPLE_FRAGMENT_SHADER_PATH))
            });
        }

        public void ResizeTextures()
        {
            PositionTexture.Resize(Resolution.Width, Resolution.Height, 0);
            PositionTexture.Bind();
            PositionTexture.ReserveMemory();

            ColorTexture.Resize(Resolution.Width, Resolution.Height, 0);
            ColorTexture.Bind();
            ColorTexture.ReserveMemory();

            NormalDepthTexture.Resize(Resolution.Width, Resolution.Height, 0);
            NormalDepthTexture.Bind();
            NormalDepthTexture.ReserveMemory();

            DiffuseMaterialTexture.Resize(Resolution.Width, Resolution.Height, 0);
            DiffuseMaterialTexture.Bind();
            DiffuseMaterialTexture.ReserveMemory();

            SpecularTexture.Resize(Resolution.Width, Resolution.Height, 0);
            SpecularTexture.Bind();
            SpecularTexture.ReserveMemory();

            VelocityTexture.Resize(Resolution.Width, Resolution.Height, 0);
            VelocityTexture.Bind();
            VelocityTexture.ReserveMemory();

            FinalTexture.Resize(Resolution.Width, Resolution.Height, 0);
            FinalTexture.Bind();
            FinalTexture.ReserveMemory();

            DepthStencilTexture.Resize(Resolution.Width, Resolution.Height, 0);
            DepthStencilTexture.Bind();
            DepthStencilTexture.ReserveMemory();
        }

        protected void LoadBuffers()
        {
            PositionTexture = new Texture(Resolution.Width, Resolution.Height, 0)
            {
                Target = TextureTarget.Texture2D,
                EnableMipMap = false,
                EnableAnisotropy = false,
                PixelInternalFormat = PixelInternalFormat.Rgb16f,
                PixelFormat = PixelFormat.Rgb,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            PositionTexture.Bind();
            PositionTexture.ReserveMemory();

            ColorTexture = new Texture(Resolution.Width, Resolution.Height, 0)
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
            ColorTexture.Bind();
            ColorTexture.ReserveMemory();

            NormalDepthTexture = new Texture(Resolution.Width, Resolution.Height, 0)
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
            NormalDepthTexture.Bind();
            NormalDepthTexture.ReserveMemory();

            DiffuseMaterialTexture = new Texture(Resolution.Width, Resolution.Height, 0)
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
            DiffuseMaterialTexture.Bind();
            DiffuseMaterialTexture.ReserveMemory();

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
            SpecularTexture.ReserveMemory();

            VelocityTexture = new Texture(Resolution.Width, Resolution.Height, 0)
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
            VelocityTexture.Bind();
            VelocityTexture.ReserveMemory();

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

            DepthStencilTexture = new Texture(Resolution.Width, Resolution.Height, 0)
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

            GBuffer.Clear();
            GBuffer.Add(FramebufferAttachment.ColorAttachment0, PositionTexture);
            GBuffer.Add(FramebufferAttachment.ColorAttachment1, ColorTexture);
            GBuffer.Add(FramebufferAttachment.ColorAttachment2, NormalDepthTexture);
            GBuffer.Add(FramebufferAttachment.ColorAttachment3, DiffuseMaterialTexture);
            GBuffer.Add(FramebufferAttachment.ColorAttachment4, SpecularTexture);
            GBuffer.Add(FramebufferAttachment.ColorAttachment5, VelocityTexture);
            GBuffer.Add(FramebufferAttachment.ColorAttachment6, FinalTexture);
            GBuffer.Add(FramebufferAttachment.DepthStencilAttachment, DepthStencilTexture);

            GBuffer.Bind();
            GBuffer.AttachAttachments();
            GBuffer.Unbind();

            LoadPointLightMesh();
        }

        private void LoadPointLightMesh()
        {
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
                    /*new Vector3(-0.5f, -0.5f, -0.5f),
                    new Vector3(-0.5f, 0.5f, -0.5f),
                    new Vector3(-0.5f, -0.5f, 0.5f),
                    new Vector3(-0.5f, 0.5f, 0.5f),
                    new Vector3(0.5f, -0.5f, -0.5f),
                    new Vector3(0.5f, 0.5f, -0.5f),
                    new Vector3(0.5f, -0.5f, 0.5f),
                    new Vector3(0.5f, 0.5f, 0.5f)*/
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
                    /*7, 6, 4,
                    7, 4, 5,
                    1, 3, 7,
                    1, 7, 5,
                    3, 2, 6,
                    3, 6, 7,
                    1, 0, 2,
                    1, 2, 3,
                    2, 0, 4,
                    2, 4, 6,
                    5, 4, 0,
                    5, 0, 1*/
                },
                _pointLightProgram
            );
        }

        public void GeometryPass(TextureManager textureManager, Camera camera, IEnumerable<Brush> brushes, IEnumerable<GameObject> gameObjects)
        {
            // Clear final texture from last frame
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, GBuffer._handle);
            GL.DrawBuffer(DrawBufferMode.ColorAttachment6);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            _geometryProgram.Use();
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, GBuffer._handle);
            GL.DrawBuffers(7, new DrawBuffersEnum[]
            {
                DrawBuffersEnum.ColorAttachment0,
                DrawBuffersEnum.ColorAttachment1,
                DrawBuffersEnum.ColorAttachment2,
                DrawBuffersEnum.ColorAttachment3,
                DrawBuffersEnum.ColorAttachment4,
                DrawBuffersEnum.ColorAttachment5,
                DrawBuffersEnum.ColorAttachment6
            });

            GL.ClearColor(Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);

            camera.Draw(_geometryProgram);

            foreach (var brush in brushes)
            {
                BindTextures(textureManager, brush.TextureMapping);
                brush.Draw(_geometryProgram);
            }

            foreach (var gameObject in gameObjects.Where(g => g.Mesh != null))
            {
                BindTextures(textureManager, gameObject.TextureMapping);
                gameObject.Draw(_geometryProgram);
            }

            /*GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, GBuffer._handle);
            GL.DrawBuffer(DrawBufferMode.ColorAttachment0);
            //GL.Clear(ClearBufferMask.ColorBufferBit);
            //GL.Disable(EnableCap.DepthTest);
            _simpleProgram.Use();
            camera.Draw(_simpleProgram);

            var translation = new Vector3(0.0f, 0.0f, -1.0f);
            var rotation = Quaternion.Identity;
            var scale = Vector3.One;

            _simpleProgram.SetUniform("modelMatrix", Matrix4.Identity * Matrix4.CreateScale(scale) * Matrix4.CreateFromQuaternion(rotation) * Matrix4.CreateTranslation(translation));
            _pointLightMesh.Draw();*/
        }

        public void StencilPass(PointLight light, Camera camera)
        {
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, GBuffer._handle);
            GL.DrawBuffer(DrawBufferMode.None);

            GL.DepthMask(false);
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.CullFace);

            GL.Clear(ClearBufferMask.StencilBufferBit);

            GL.StencilFunc(StencilFunction.Always, 0, 0);
            GL.StencilOpSeparate(StencilFace.Back, StencilOp.Keep, StencilOp.IncrWrap, StencilOp.Keep);
            GL.StencilOpSeparate(StencilFace.Front, StencilOp.Keep, StencilOp.DecrWrap, StencilOp.Keep);

            _stencilProgram.Use();
            camera.Draw(_stencilProgram);
            light.Draw(_stencilProgram);
            _pointLightMesh.Draw();
        }

        public void LightPass(Camera camera, IEnumerable<PointLight> pointLights)
        {
            GL.Enable(EnableCap.StencilTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendEquation(BlendEquationMode.FuncAdd);
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.One);

            foreach (var light in pointLights)
            {
                StencilPass(light, camera);

                _pointLightProgram.Use();
                _pointLightProgram.BindTexture(PositionTexture, "positionMap", 0);
                _pointLightProgram.BindTexture(ColorTexture, "colorMap", 1);
                _pointLightProgram.BindTexture(NormalDepthTexture, "normalDepth", 2);
                _pointLightProgram.BindTexture(DiffuseMaterialTexture, "diffuseMaterial", 3);
                _pointLightProgram.BindTexture(SpecularTexture, "specularMap", 4);

                camera.Draw(_pointLightProgram);

                GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, GBuffer._handle);
                GL.DrawBuffer(DrawBufferMode.ColorAttachment6);

                GL.StencilFunc(StencilFunction.Notequal, 0, 0xFF);
                GL.Disable(EnableCap.DepthTest);
                GL.Enable(EnableCap.CullFace);
                GL.CullFace(CullFaceMode.Front);

                light.Draw(_pointLightProgram);
                _pointLightMesh.Draw();
                //RenderQuad();
            }

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            GL.Disable(EnableCap.StencilTest);
            GL.Disable(EnableCap.Blend);
        }

        protected void LoadQuad(ShaderProgram program)
        {
            _vertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayHandle);
            _vertexBuffer.AddVertices(new[]
            {
                /*new Vector3(1.0f, 1.0f, 0.0f),
                new Vector3(-1.0f, 1.0f, 0.0f),
                new Vector3(-1.0f, -1.0f, 0.0f),
                new Vector3(1.0f, -1.0f, 0.0f)*/
                new Vector3(0.5f, 0.5f, 0.0f),
                new Vector3(-0.5f, 0.5f, 0.0f),
                new Vector3(-0.5f, -0.5f, 0.0f),
                new Vector3(0.5f, -0.5f, 0.0f)
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

        private void BindTextures(TextureManager textureManager, TextureMapping textureMapping)
        {
            // TODO - Order brush rendering in a way that allows us to not re-bind duplicate textures repeatedly
            // Check brush's texture mapping to see which textures we need to bind
            var mainTexture = textureManager.RetrieveTexture(textureMapping.MainTextureID);

            GL.Uniform1(_geometryProgram.GetUniformLocation("useMainTexture"), (mainTexture != null) ? 1 : 0);
            if (mainTexture != null)
            {
                _geometryProgram.BindTexture(mainTexture, "mainTexture", 0);
            }

            var normalMap = textureManager.RetrieveTexture(textureMapping.NormalMapID);

            GL.Uniform1(_geometryProgram.GetUniformLocation("useNormalMap"), (normalMap != null) ? 1 : 0);
            if (normalMap != null)
            {
                _geometryProgram.BindTexture(normalMap, "normalMap", 1);
            }

            var diffuseMap = textureManager.RetrieveTexture(textureMapping.DiffuseMapID);

            GL.Uniform1(_geometryProgram.GetUniformLocation("useDiffuseMap"), (diffuseMap != null) ? 1 : 0);
            if (diffuseMap != null)
            {
                _geometryProgram.BindTexture(diffuseMap, "diffuseMap", 2);
            }

            var specularMap = textureManager.RetrieveTexture(textureMapping.SpecularMapID);

            GL.Uniform1(_geometryProgram.GetUniformLocation("useSpecularMap"), (specularMap != null) ? 1 : 0);
            if (specularMap != null)
            {
                _geometryProgram.BindTexture(specularMap, "specularMap", 3);
            }
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
