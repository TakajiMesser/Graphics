using Graphics.GameObjects;
using Graphics.Helpers;
using Graphics.Meshes;
using Graphics.Outputs;
using Graphics.Rendering.Buffers;
using Graphics.Rendering.Shaders;
using Graphics.Rendering.Textures;
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
    public class GeometryRenderer
    {
        public Resolution Resolution { get; private set; }
        public Texture FinalTexture { get; protected set; }
        public Texture DepthTexture { get; protected set; }

        internal ShaderProgram _geometryProgram;
        protected FrameBuffer _frameBuffer = new FrameBuffer();

        public GeometryRenderer(Resolution resolution)
        {
            Resolution = resolution;
        }

        public void Load()
        {
            LoadPrograms();
            LoadBuffers();
        }

        protected void LoadPrograms()
        {
            _geometryProgram = new ShaderProgram(new[] {
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.GEOMETRY_VERTEX_SHADER_PATH)),
                new Shader(ShaderType.TessControlShader, File.ReadAllText(FilePathHelper.GEOMETRY_TESS_CONTROL_SHADER_PATH)),
                new Shader(ShaderType.TessEvaluationShader, File.ReadAllText(FilePathHelper.GEOMETRY_TESS_EVAL_SHADER_PATH)),
                new Shader(ShaderType.GeometryShader, File.ReadAllText(FilePathHelper.GEOMETRY_GEOMETRY_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.GEOMETRY_FRAGMENT_SHADER_PATH))
            });
        }

        protected void LoadBuffers()
        {
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

            _frameBuffer.Add(FramebufferAttachment.ColorAttachment0, FinalTexture);
            _frameBuffer.Add(FramebufferAttachment.DepthAttachment, DepthTexture);

            _frameBuffer.Bind();
            _frameBuffer.AttachAttachments();
            _frameBuffer.Unbind();
        }

        public void Render(TextureManager textureManager, Camera camera, IEnumerable<Brush> brushes, IEnumerable<GameObject> gameObjects)
        {
            _geometryProgram.Use();
            _frameBuffer.Draw();

            GL.ClearColor(Color4.Purple);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            camera.Draw(_geometryProgram);

            foreach (var brush in brushes)
            {
                // TODO - Order brush rendering in a way that allows us to not re-bind duplicate textures repeatedly
                // Check brush's texture mapping to see which textures we need to bind
                var mainTexture = textureManager.RetrieveTexture(brush.TextureMapping.MainTextureID);

                GL.Uniform1(_geometryProgram.GetUniformLocation("useMainTexture"), (mainTexture != null) ? 1 : 0);
                if (mainTexture != null)
                {
                    _geometryProgram.BindTexture(mainTexture, "mainTexture", 0);
                }

                var normalMap = textureManager.RetrieveTexture(brush.TextureMapping.NormalMapID);

                GL.Uniform1(_geometryProgram.GetUniformLocation("useNormalMap"), (normalMap != null) ? 1 : 0);
                if (normalMap != null)
                {
                    _geometryProgram.BindTexture(normalMap, "normalMap", 1);
                }

                var diffuseMap = textureManager.RetrieveTexture(brush.TextureMapping.DiffuseMapID);

                GL.Uniform1(_geometryProgram.GetUniformLocation("useDiffuseMap"), (diffuseMap != null) ? 1 : 0);
                if (diffuseMap != null)
                {
                    _geometryProgram.BindTexture(diffuseMap, "diffuseMap", 2);
                }

                var specularMap = textureManager.RetrieveTexture(brush.TextureMapping.SpecularMapID);

                GL.Uniform1(_geometryProgram.GetUniformLocation("useSpecularMap"), (specularMap != null) ? 1 : 0);
                if (specularMap != null)
                {
                    _geometryProgram.BindTexture(specularMap, "specularMap", 3);
                }

                brush.Draw(_geometryProgram);
            }

            foreach (var gameObject in gameObjects.Where(g => g.Mesh != null))
            {
                // TODO - Order gameobject rendering in a way that allows us to not re-bind duplicate textures repeatedly
                // Check game object's texture mapping to see which textures we need to bind
                var mainTexture = textureManager.RetrieveTexture(gameObject.TextureMapping.MainTextureID);

                GL.Uniform1(_geometryProgram.GetUniformLocation("useMainTexture"), (mainTexture != null) ? 1 : 0);
                if (mainTexture != null)
                {
                    _geometryProgram.BindTexture(mainTexture, "mainTexture", 0);
                }

                var normalMap = textureManager.RetrieveTexture(gameObject.TextureMapping.NormalMapID);

                GL.Uniform1(_geometryProgram.GetUniformLocation("useNormalMap"), (normalMap != null) ? 1 : 0);
                if (normalMap != null)
                {
                    _geometryProgram.BindTexture(normalMap, "normalMap", 1);
                }

                var diffuseMap = textureManager.RetrieveTexture(gameObject.TextureMapping.DiffuseMapID);

                GL.Uniform1(_geometryProgram.GetUniformLocation("useDiffuseMap"), (diffuseMap != null) ? 1 : 0);
                if (diffuseMap != null)
                {
                    _geometryProgram.BindTexture(diffuseMap, "diffuseMap", 2);
                }

                var specularMap = textureManager.RetrieveTexture(gameObject.TextureMapping.SpecularMapID);

                GL.Uniform1(_geometryProgram.GetUniformLocation("useSpecularMap"), (specularMap != null) ? 1 : 0);
                if (specularMap != null)
                {
                    _geometryProgram.BindTexture(specularMap, "specularMap", 3);
                }

                gameObject.Draw(_geometryProgram);
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
