using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TakoEngine.Entities;
using TakoEngine.Entities.Cameras;
using TakoEngine.Entities.Lights;
using TakoEngine.Entities.Models;
using TakoEngine.Helpers;
using TakoEngine.Outputs;
using TakoEngine.Rendering.Buffers;
using TakoEngine.Rendering.Shaders;
using TakoEngine.Rendering.Textures;

namespace TakoEngine.Rendering.Processing
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

        public void PointLightPass(Camera camera, PointLight light, IEnumerable<Brush> brushes, IEnumerable<Actor> actors)
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
            camera.DrawFromLight(_pointShadowProgram, light);
            _pointShadowProgram.SetUniform("lightRadius", light.Radius);
            _pointShadowProgram.SetUniform("lightPosition", light.Position);
            // Draw all geometry, but only the positions
            foreach (var brush in brushes)
            {
                brush.Draw(_pointShadowProgram);
            }

            foreach (var actor in actors)
            {
                actor.Draw(_pointShadowProgram);
            }

            _pointFrameBuffer.Unbind(FramebufferTarget.DrawFramebuffer);
        }

        public void PointLightJointPass(Camera camera, PointLight light, IEnumerable<Actor> actors)
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
            camera.DrawFromLight(_pointShadowJointProgram, light);
            _pointShadowJointProgram.SetUniform("lightRadius", light.Radius);
            _pointShadowJointProgram.SetUniform("lightPosition", light.Position);
            // Draw all geometry, but only the positions
            foreach (var actor in actors)
            {
                actor.Draw(_pointShadowJointProgram);
            }

            _pointFrameBuffer.Unbind(FramebufferTarget.DrawFramebuffer);
        }

        private void SpotLightPass(Camera camera, SpotLight light, IEnumerable<Brush> brushes, IEnumerable<Actor> actors)
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

            // Draw camera from the spot light's perspective
            camera.DrawFromLight(_spotShadowProgram, light);

            // Draw all geometry, but only the positions
            foreach (var brush in brushes)
            {
                brush.Draw(_spotShadowProgram);
            }

            foreach (var actor in actors)
            {
                actor.Draw(_spotShadowProgram);
            }

            _spotFrameBuffer.Unbind(FramebufferTarget.DrawFramebuffer);
        }

        private void SpotLightJointPass(Camera camera, SpotLight light, IEnumerable<Actor> actors)
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

            // Draw camera from the spot light's perspective
            camera.DrawFromLight(_spotShadowJointProgram, light);

            // Draw all geometry, but only the positions
            foreach (var actor in actors)
            {
                actor.Draw(_spotShadowJointProgram);
            }

            _spotFrameBuffer.Unbind(FramebufferTarget.DrawFramebuffer);
        }

        public void Render(Camera camera, Light light, IEnumerable<Brush> brushes, IEnumerable<Actor> actors)
        {
            switch (light)
            {
                case PointLight pLight:
                    PointLightPass(camera, pLight, brushes, actors.Where(g => g.Model is SimpleModel));
                    PointLightJointPass(camera, pLight, actors.Where(g => g.Model is AnimatedModel));
                    break;
                case SpotLight sLight:
                    SpotLightPass(camera, sLight, brushes, actors.Where(g => g.Model is SimpleModel));
                    SpotLightJointPass(camera, sLight, actors.Where(g => g.Model is AnimatedModel));
                    break;
            }
        }
    }
}
