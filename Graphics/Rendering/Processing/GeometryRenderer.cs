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
            _geometryProgram = new ShaderProgram(new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.VERTEX_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.FRAGMENT_SHADER_PATH)));
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

            _frameBuffer.Add(FramebufferAttachment.ColorAttachment0, FinalTexture);
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
