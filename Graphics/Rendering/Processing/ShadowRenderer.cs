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
    public class ShadowRenderer : Renderer
    {
        public const int SHADOW_SIZE = 1024;

        public Texture PointDepthCubeMap { get; protected set; }
        public Texture SpotDepthTexture { get; protected set; }

        internal ShaderProgram _pointShadowProgram;
        internal ShaderProgram _pointShadowJointProgram;
        internal ShaderProgram _spotShadowProgram;
        internal ShaderProgram _spotShadowJointProgram;

        internal FrameBuffer _pointFrameBuffer = new FrameBuffer();
        internal FrameBuffer _spotFrameBuffer = new FrameBuffer();

        protected override void LoadPrograms()
        {
            _pointShadowProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.POINT_SHADOW_VERTEX_SHADER_PATH)),
                new Shader(ShaderType.GeometryShader, File.ReadAllText(FilePathHelper.POINT_SHADOW_GEOMETRY_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.POINT_SHADOW_FRAGMENT_SHADER_PATH))
            );

            _pointShadowJointProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.POINT_SHADOW_SKINNING_VERTEX_SHADER_PATH)),
                new Shader(ShaderType.GeometryShader, File.ReadAllText(FilePathHelper.POINT_SHADOW_GEOMETRY_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.POINT_SHADOW_FRAGMENT_SHADER_PATH))
            );

            _spotShadowProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.SPOT_SHADOW_VERTEX_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.SPOT_SHADOW_FRAGMENT_SHADER_PATH))
            );

            _spotShadowJointProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.SPOT_SHADOW_SKINNING_VERTEX_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.SPOT_SHADOW_FRAGMENT_SHADER_PATH))
            );
        }

        public override void ResizeTextures(Resolution resolution)
        {
            PointDepthCubeMap.Resize(SHADOW_SIZE, SHADOW_SIZE, 0);
            PointDepthCubeMap.Bind();
            PointDepthCubeMap.ReserveMemory();

            SpotDepthTexture.Resize(resolution.Width, resolution.Height, 0);
            SpotDepthTexture.Bind();
            SpotDepthTexture.ReserveMemory();
        }

        protected override void LoadTextures(Resolution resolution)
        {
            PointDepthCubeMap = new Texture(SHADOW_SIZE, SHADOW_SIZE, 6)
            {
                Target = TextureTarget.TextureCubeMap,
                EnableMipMap = false,
                EnableAnisotropy = false,
                PixelInternalFormat = PixelInternalFormat.DepthComponent,
                PixelFormat = PixelFormat.DepthComponent,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.ClampToEdge
            };
            PointDepthCubeMap.Bind();
            PointDepthCubeMap.ReserveMemory();

            SpotDepthTexture = new Texture(resolution.Width, resolution.Height, 0)
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
        }

        protected override void LoadBuffers()
        {
            _pointFrameBuffer.Clear();
            _pointFrameBuffer.Add(FramebufferAttachment.DepthAttachment, PointDepthCubeMap);

            _pointFrameBuffer.Bind(FramebufferTarget.Framebuffer);
            _pointFrameBuffer.AttachAttachments();
            _pointFrameBuffer.Unbind(FramebufferTarget.Framebuffer);

            _spotFrameBuffer.Clear();
            _spotFrameBuffer.Add(FramebufferAttachment.DepthAttachment, SpotDepthTexture);

            _spotFrameBuffer.Bind(FramebufferTarget.Framebuffer);
            _spotFrameBuffer.AttachAttachments();
            _spotFrameBuffer.Unbind(FramebufferTarget.Framebuffer);
        }

        public void PointLightPass(Camera camera, PointLight light, IEnumerable<Brush> brushes, IEnumerable<GameObject> gameObjects)
        {
            _pointShadowProgram.Use();

            _pointFrameBuffer.Bind(FramebufferTarget.DrawFramebuffer);
            _pointFrameBuffer.Draw(DrawBuffersEnum.None);

            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.StencilTest);
            
            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, SHADOW_SIZE, SHADOW_SIZE);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.StencilFunc(StencilFunction.Notequal, 0, 0xFF);

            // Draw camera from the point light's perspective
            //GL.FramebufferTexture(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.DepthAttachment, PointDepthCubeMap.Handle, 0);
            camera.DrawFromLight(_pointShadowProgram, light);
            _pointShadowProgram.SetUniform("lightRadius", light.Radius);
            _pointShadowProgram.SetUniform("lightPosition", light.Position);
            // Draw all geometry, but only the positions
            foreach (var brush in brushes)
            {
                brush.Draw(_pointShadowProgram);
            }

            foreach (var gameObject in gameObjects)
            {
                gameObject.Draw(_pointShadowProgram);
            }

            _pointFrameBuffer.Unbind(FramebufferTarget.DrawFramebuffer);
        }

        public void PointLightJointPass(Camera camera, PointLight light, IEnumerable<GameObject> gameObjects)
        {
            _pointShadowJointProgram.Use();

            _pointFrameBuffer.Bind(FramebufferTarget.DrawFramebuffer);
            _pointFrameBuffer.Draw(DrawBuffersEnum.None);

            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.StencilTest);

            //GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, SHADOW_SIZE, SHADOW_SIZE);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.StencilFunc(StencilFunction.Notequal, 0, 0xFF);

            // Draw camera from the point light's perspective
            //GL.FramebufferTexture(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.DepthAttachment, PointDepthCubeMap.Handle, 0);
            camera.DrawFromLight(_pointShadowJointProgram, light);
            _pointShadowJointProgram.SetUniform("lightRadius", light.Radius);
            _pointShadowJointProgram.SetUniform("lightPosition", light.Position);
            // Draw all geometry, but only the positions
            foreach (var gameObject in gameObjects)
            {
                gameObject.Draw(_pointShadowJointProgram);
            }

            _pointFrameBuffer.Unbind(FramebufferTarget.DrawFramebuffer);
        }

        private void SpotLightPass(Resolution resolution, Camera camera, SpotLight light, IEnumerable<Brush> brushes, IEnumerable<GameObject> gameObjects)
        {
            _spotShadowProgram.Use();

            _spotFrameBuffer.Bind(FramebufferTarget.DrawFramebuffer);
            _spotFrameBuffer.Draw(DrawBuffersEnum.None);

            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.StencilTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.StencilFunc(StencilFunction.Notequal, 0, 0xFF);
            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, resolution.Width, resolution.Height);

            // Draw camera from the spot light's perspective
            camera.DrawFromLight(_spotShadowProgram, light);

            // Draw all geometry, but only the positions
            foreach (var brush in brushes)
            {
                brush.Draw(_spotShadowProgram);
            }

            foreach (var gameObject in gameObjects)
            {
                gameObject.Draw(_spotShadowProgram);
            }
        }

        private void SpotLightJointPass(Resolution resolution, Camera camera, SpotLight light, IEnumerable<GameObject> gameObjects)
        {
            _spotShadowJointProgram.Use();

            _spotFrameBuffer.Bind(FramebufferTarget.DrawFramebuffer);
            _spotFrameBuffer.Draw(DrawBuffersEnum.None);

            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.StencilTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.StencilFunc(StencilFunction.Notequal, 0, 0xFF);
            //GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, resolution.Width, resolution.Height);

            // Draw camera from the spot light's perspective
            camera.DrawFromLight(_spotShadowJointProgram, light);

            // Draw all geometry, but only the positions
            foreach (var gameObject in gameObjects)
            {
                gameObject.Draw(_spotShadowJointProgram);
            }
        }

        private void PointLightPass(Camera camera, IEnumerable<PointLight> lights, IEnumerable<Brush> brushes, IEnumerable<GameObject> gameObjects)
        {
            _pointShadowProgram.Use();

            _pointFrameBuffer.Bind(FramebufferTarget.DrawFramebuffer);
            _pointFrameBuffer.Draw(DrawBuffersEnum.None);

            GL.Enable(EnableCap.DepthTest);
            //GL.DepthFunc(DepthFunction.Less);
            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, SHADOW_SIZE, SHADOW_SIZE);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);

            // Draw camera from the point light's perspective
            foreach (var light in lights)
            {
                //GL.FramebufferTexture(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.DepthAttachment, PointDepthCubeMap.Handle, 0);
                //GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.TextureCubeMapPositiveX + i, PointDepthCubeMap.Handle, 0);
                camera.DrawFromLight(_pointShadowProgram, light);
                _pointShadowProgram.SetUniform("farPlane", light.Radius);
                _pointShadowProgram.SetUniform("lightPosition", light.Position);
                // Draw all geometry, but only the positions
                foreach (var brush in brushes)
                {
                    brush.Draw(_pointShadowProgram);
                }

                foreach (var gameObject in gameObjects)
                {
                    gameObject.Draw(_pointShadowProgram);
                }
            }

            _pointFrameBuffer.Unbind(FramebufferTarget.DrawFramebuffer);
        }

        private void SpotLightPass(Resolution resolution, Camera camera, IEnumerable<SpotLight> lights, IEnumerable<Brush> brushes, IEnumerable<GameObject> gameObjects)
        {
            _spotShadowProgram.Use();

            _spotFrameBuffer.Bind(FramebufferTarget.DrawFramebuffer);
            _spotFrameBuffer.Draw(DrawBuffersEnum.None);

            GL.Enable(EnableCap.DepthTest);
            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, resolution.Width, resolution.Height);

            // Draw camera from the spot light's perspective
            foreach (var light in lights)
            {
                camera.DrawFromLight(_spotShadowProgram, light);

                // Draw all geometry, but only the positions
                foreach (var brush in brushes)
                {
                    brush.Draw(_spotShadowProgram);
                }

                foreach (var gameObject in gameObjects)
                {
                    gameObject.Draw(_spotShadowProgram);
                }
            }
        }

        public void Render(Resolution resolution, Camera camera, Light light, IEnumerable<Brush> brushes, IEnumerable<GameObject> gameObjects)
        {
            switch (light)
            {
                case PointLight pLight:
                    PointLightPass(camera, pLight, brushes, gameObjects.Where(g => g.Model is SimpleModel));
                    PointLightJointPass(camera, pLight, gameObjects.Where(g => g.Model is AnimatedModel));
                    break;
                case SpotLight sLight:
                    SpotLightPass(resolution, camera, sLight, brushes, gameObjects.Where(g => g.Model is SimpleModel));
                    SpotLightJointPass(resolution, camera, sLight, gameObjects.Where(g => g.Model is AnimatedModel));
                    break;
            }
        }

        public void Render(Resolution resolution, Camera camera, IEnumerable<Light> lights, IEnumerable<Brush> brushes, IEnumerable<GameObject> gameObjects)
        {
            PointLightPass(camera,
                lights.Where(l => l is PointLight)
                    .Select(l => l as PointLight),
                brushes, gameObjects);

            SpotLightPass(resolution, camera,
                lights.Where(l => l is SpotLight)
                    .Select(l => l as SpotLight),
                brushes, gameObjects);
        }
    }
}
