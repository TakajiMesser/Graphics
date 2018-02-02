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
    public class ShadowRenderer
    {
        public Resolution Resolution { get; private set; }

        public Texture PointDepthTexture { get; protected set; }
        public Texture PointDepthCubeMap { get; protected set; }
        public Texture SpotDepthTexture { get; protected set; }

        internal ShaderProgram _pointShadowProgram;
        internal ShaderProgram _spotShadowProgram;

        internal FrameBuffer _pointFrameBuffer = new FrameBuffer();
        internal FrameBuffer _spotFrameBuffer = new FrameBuffer();

        public ShadowRenderer(Resolution resolution)
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
            _pointShadowProgram = new ShaderProgram(new[] {
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.POINT_SHADOW_VERTEX_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.POINT_SHADOW_FRAGMENT_SHADER_PATH))
            });

            _spotShadowProgram = new ShaderProgram(new[] {
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.SPOT_SHADOW_VERTEX_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.SPOT_SHADOW_FRAGMENT_SHADER_PATH))
            });
        }

        public void ResizeTextures()
        {
            var dimension = Math.Max(Resolution.Width, Resolution.Height);

            PointDepthTexture.Resize(Resolution.Width, Resolution.Height, 0);
            PointDepthTexture.Bind();
            PointDepthTexture.ReserveMemory();

            PointDepthCubeMap.Resize(Resolution.Width, Resolution.Height, 0);
            PointDepthCubeMap.Bind();
            PointDepthCubeMap.ReserveMemory();

            SpotDepthTexture.Resize(Resolution.Width, Resolution.Height, 0);
            SpotDepthTexture.Bind();
            SpotDepthTexture.ReserveMemory();
        }

        protected void LoadBuffers()
        {
            PointDepthTexture = new Texture(Resolution.Width, Resolution.Height, 0)
            {
                Target = TextureTarget.Texture2D,
                EnableMipMap = false,
                EnableAnisotropy = false,
                PixelInternalFormat = PixelInternalFormat.DepthComponent32f,
                PixelFormat = PixelFormat.DepthComponent,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.ClampToEdge
            };
            PointDepthTexture.Bind();
            PointDepthTexture.ReserveMemory();

            PointDepthCubeMap = new Texture(Resolution.Width, Resolution.Height, 0)
            {
                Target = TextureTarget.TextureCubeMap,
                EnableMipMap = false,
                EnableAnisotropy = false,
                PixelInternalFormat = PixelInternalFormat.DepthComponent32f,
                PixelFormat = PixelFormat.DepthComponent,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.ClampToEdge
            };
            PointDepthCubeMap.Bind();
            PointDepthCubeMap.ReserveMemory();

            SpotDepthTexture = new Texture(Resolution.Width, Resolution.Height, 0)
            {
                Target = TextureTarget.Texture2D,
                EnableMipMap = false,
                EnableAnisotropy = false,
                PixelInternalFormat = PixelInternalFormat.DepthComponent,
                PixelFormat = PixelFormat.DepthComponent,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.ClampToEdge
            };
            SpotDepthTexture.Bind();
            SpotDepthTexture.ReserveMemory();

            _pointFrameBuffer.Clear();
            _pointFrameBuffer.Add(FramebufferAttachment.DepthAttachment, PointDepthTexture);

            for (var i = 0; i < 6; i++)
            {
                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.R32f, Resolution.Width, Resolution.Height, 0, PixelFormat.Red, PixelType.Float, IntPtr.Zero);
            }

            _pointFrameBuffer.Bind(FramebufferTarget.Framebuffer);
            _pointFrameBuffer.AttachAttachments();
            _pointFrameBuffer.Unbind(FramebufferTarget.Framebuffer);

            _spotFrameBuffer.Clear();
            _spotFrameBuffer.Add(FramebufferAttachment.DepthAttachment, SpotDepthTexture);

            _spotFrameBuffer.Bind(FramebufferTarget.Framebuffer);
            _spotFrameBuffer.AttachAttachments();
            _spotFrameBuffer.Unbind(FramebufferTarget.Framebuffer);
        }

        private void PointLightPass(Camera camera, IEnumerable<PointLight> lights, IEnumerable<Brush> brushes, IEnumerable<GameObject> gameObjects)
        {
            _pointShadowProgram.Use();

            GL.CullFace(CullFaceMode.Front);

            //_pointFrameBuffer.Bind(FramebufferTarget.DrawFramebuffer);
            //_pointFrameBuffer.Draw(DrawBuffersEnum.None);
            //GL.Enable(EnableCap.DepthTest);

            // Draw camera from the spot light's perspective
            foreach (var light in lights)
            {
                for (var i = 0; i < 6; i++)
                {
                    // Bind FBO for writing the texture target
                    _pointFrameBuffer.Bind(FramebufferTarget.DrawFramebuffer);
                    GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.TextureCubeMapPositiveX + i, PointDepthCubeMap.Handle, 0);
                    _pointFrameBuffer.Draw(DrawBuffersEnum.ColorAttachment0);

                    GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
                    GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

                    camera.DrawFromLight(_pointShadowProgram, light, TextureTarget.TextureCubeMapPositiveX + i);

                    // Draw all geometry, but only the positions
                    foreach (var brush in brushes)
                    {
                        brush.Draw(_pointShadowProgram);
                    }

                    foreach (var gameObject in gameObjects.Where(g => g.Mesh != null))
                    {
                        gameObject.Draw(_pointShadowProgram);
                    }
                }
            }
        }

        private void SpotLightPass(Camera camera, IEnumerable<SpotLight> lights, IEnumerable<Brush> brushes, IEnumerable<GameObject> gameObjects)
        {
            _spotShadowProgram.Use();

            _spotFrameBuffer.Bind(FramebufferTarget.DrawFramebuffer);
            _spotFrameBuffer.Draw(DrawBuffersEnum.None);

            GL.Enable(EnableCap.DepthTest);
            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            // Draw camera from the spot light's perspective
            foreach (var light in lights)
            {
                camera.DrawFromLight(_spotShadowProgram, light);

                // Draw all geometry, but only the positions
                foreach (var brush in brushes)
                {
                    brush.Draw(_spotShadowProgram);
                }

                foreach (var gameObject in gameObjects.Where(g => g.Mesh != null))
                {
                    gameObject.Draw(_spotShadowProgram);
                }
            }
        }

        public void Render(Camera camera, IEnumerable<Light> lights, IEnumerable<Brush> brushes, IEnumerable<GameObject> gameObjects)
        {
            PointLightPass(camera,
                lights.Where(l => l is PointLight)
                    .Select(l => l as PointLight),
                brushes, gameObjects);

            SpotLightPass(camera,
                lights.Where(l => l is SpotLight)
                    .Select(l => l as SpotLight),
                brushes, gameObjects);
        }
    }
}
