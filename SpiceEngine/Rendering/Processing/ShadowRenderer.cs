using OpenTK.Graphics.OpenGL;
using SpiceEngine.Properties;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SpiceEngine.Entities;
using SpiceEngine.Entities.Cameras;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Entities.Models;
using SpiceEngine.Helpers;
using SpiceEngine.Outputs;
using SpiceEngine.Rendering.Buffers;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;

namespace SpiceEngine.Rendering.Processing
{
    public class ShadowRenderer : Renderer
    {
        public const int SHADOW_SIZE = 1024;

        public Texture PointDepthCubeMap { get; protected set; }
        public Texture SpotDepthTexture { get; protected set; }

        private ShaderProgram _pointShadowProgram;
        private ShaderProgram _pointShadowJointProgram;
        private ShaderProgram _spotShadowProgram;
        private ShaderProgram _spotShadowJointProgram;

        private FrameBuffer _pointFrameBuffer = new FrameBuffer();
        private FrameBuffer _spotFrameBuffer = new FrameBuffer();

        protected override void LoadPrograms()
        {
            _pointShadowProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, Resources.point_shadow_vert),
                new Shader(ShaderType.GeometryShader, Resources.point_shadow_geom),
                new Shader(ShaderType.FragmentShader, Resources.point_shadow_frag)
            );

            _pointShadowJointProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, Resources.point_shadow_skinning_vert),
                new Shader(ShaderType.GeometryShader, Resources.point_shadow_geom),
                new Shader(ShaderType.FragmentShader, Resources.point_shadow_frag)
            );

            _spotShadowProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, Resources.spot_shadow_vert),
                new Shader(ShaderType.FragmentShader, Resources.spot_shadow_frag)
            );

            _spotShadowJointProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, Resources.spot_shadow_skinning_vert),
                new Shader(ShaderType.FragmentShader, Resources.spot_shadow_frag)
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

        private void BindForPointShadowDrawing()
        {
            _pointFrameBuffer.BindAndDraw(DrawBuffersEnum.None);

            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.StencilTest);

            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, SHADOW_SIZE, SHADOW_SIZE);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.StencilFunc(StencilFunction.Notequal, 0, 0xFF);
        }

        private void BindForSpotShadowDrawing()
        {
            _spotFrameBuffer.BindAndDraw(DrawBuffersEnum.None);

            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.StencilTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.StencilFunc(StencilFunction.Notequal, 0, 0xFF);
            GL.Clear(ClearBufferMask.DepthBufferBit);
        }

        private void PointLightPass(Camera camera, PointLight light, IEnumerable<Brush> brushes, IEnumerable<Actor> actors)
        {
            _pointShadowProgram.Use();

            // Draw camera from the point light's perspective
            camera.SetUniforms(_pointShadowProgram, light);
            _pointShadowProgram.SetUniform("lightRadius", light.Radius);
            _pointShadowProgram.SetUniform("lightPosition", light.Position);

            // Draw all geometry, but only the positions
            foreach (var brush in brushes)
            {
                brush.SetUniforms(_pointShadowProgram);
                brush.Draw();
            }

            foreach (var actor in actors)
            {
                actor.SetUniformsAndDraw(_pointShadowProgram);
            }
        }

        private void PointLightJointPass(Camera camera, PointLight light, IEnumerable<Actor> actors)
        {
            _pointShadowJointProgram.Use();

            // Draw camera from the point light's perspective
            camera.SetUniforms(_pointShadowJointProgram, light);
            _pointShadowJointProgram.SetUniform("lightRadius", light.Radius);
            _pointShadowJointProgram.SetUniform("lightPosition", light.Position);

            // Draw all geometry, but only the positions
            foreach (var actor in actors)
            {
                actor.SetUniformsAndDraw(_pointShadowJointProgram);
            }

            _pointFrameBuffer.Unbind(FramebufferTarget.DrawFramebuffer);
        }

        private void SpotLightPass(Camera camera, SpotLight light, IEnumerable<Brush> brushes, IEnumerable<Actor> actors)
        {
            _spotShadowProgram.Use();

            // Draw camera from the spot light's perspective
            camera.SetUniforms(_spotShadowProgram, light);

            // Draw all geometry, but only the positions
            foreach (var brush in brushes)
            {
                brush.SetUniforms(_spotShadowProgram);
                brush.Draw();
            }

            foreach (var actor in actors)
            {
                actor.SetUniformsAndDraw(_spotShadowProgram);
            }
        }

        private void SpotLightJointPass(Camera camera, SpotLight light, IEnumerable<Actor> actors)
        {
            _spotShadowJointProgram.Use();

            // Draw camera from the spot light's perspective
            camera.SetUniforms(_spotShadowJointProgram, light);

            // Draw all geometry, but only the positions
            foreach (var actor in actors)
            {
                actor.SetUniformsAndDraw(_spotShadowJointProgram);
            }

            _spotFrameBuffer.Unbind(FramebufferTarget.DrawFramebuffer);
        }

        public void Render(Camera camera, Light light, IEnumerable<Brush> brushes, IEnumerable<Actor> actors)
        {
            switch (light)
            {
                case PointLight pLight:
                    BindForPointShadowDrawing();
                    PointLightPass(camera, pLight, brushes, actors.Where(g => g.Model is Model3D<Vertex3D>));
                    PointLightJointPass(camera, pLight, actors.Where(g => g.Model is AnimatedModel3D));
                    break;
                case SpotLight sLight:
                    BindForSpotShadowDrawing();
                    SpotLightPass(camera, sLight, brushes, actors.Where(g => g.Model is Model3D<Vertex3D>));
                    SpotLightJointPass(camera, sLight, actors.Where(g => g.Model is AnimatedModel3D));
                    break;
            }
        }
    }
}
