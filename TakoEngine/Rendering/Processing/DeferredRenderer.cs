﻿using OpenTK;
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
    public class DeferredRenderer : Renderer
    {
        public Texture PositionTexture { get; protected set; }
        public Texture ColorTexture { get; protected set; }
        public Texture NormalTexture { get; protected set; }
        public Texture DiffuseMaterialTexture { get; protected set; }
        public Texture SpecularTexture { get; protected set; }
        public Texture VelocityTexture { get; protected set; }
        public Texture FinalTexture { get; protected set; }
        public Texture DepthStencilTexture { get; protected set; }

        public FrameBuffer GBuffer { get; private set; } = new FrameBuffer();

        internal ShaderProgram _geometryProgram;
        internal ShaderProgram _jointGeometryProgram;

        protected override void LoadPrograms()
        {
            _geometryProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.DEFERRED_VERTEX_SHADER_PATH)),
                //new Shader(ShaderType.TessControlShader, File.ReadAllText(FilePathHelper.DEFERRED_TESS_CONTROL_SHADER_PATH)),
                //new Shader(ShaderType.TessEvaluationShader, File.ReadAllText(FilePathHelper.DEFERRED_TESS_EVAL_SHADER_PATH)),
                //new Shader(ShaderType.GeometryShader, File.ReadAllText(FilePathHelper.DEFERRED_GEOMETRY_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.DEFERRED_FRAGMENT_SHADER_PATH))
            );

            _jointGeometryProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.DEFERRED_SKINNING_VERTEX_SHADER_PATH)),
                //new Shader(ShaderType.TessControlShader, File.ReadAllText(FilePathHelper.DEFERRED_TESS_CONTROL_SHADER_PATH)),
                //new Shader(ShaderType.TessEvaluationShader, File.ReadAllText(FilePathHelper.DEFERRED_TESS_EVAL_SHADER_PATH)),
                //new Shader(ShaderType.GeometryShader, File.ReadAllText(FilePathHelper.DEFERRED_GEOMETRY_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.DEFERRED_FRAGMENT_SHADER_PATH))
            );
        }

        public override void ResizeTextures(Resolution resolution)
        {
            PositionTexture.Resize(resolution.Width, resolution.Height, 0);
            PositionTexture.Bind();
            PositionTexture.ReserveMemory();

            ColorTexture.Resize(resolution.Width, resolution.Height, 0);
            ColorTexture.Bind();
            ColorTexture.ReserveMemory();

            NormalTexture.Resize(resolution.Width, resolution.Height, 0);
            NormalTexture.Bind();
            NormalTexture.ReserveMemory();

            DiffuseMaterialTexture.Resize(resolution.Width, resolution.Height, 0);
            DiffuseMaterialTexture.Bind();
            DiffuseMaterialTexture.ReserveMemory();

            SpecularTexture.Resize(resolution.Width, resolution.Height, 0);
            SpecularTexture.Bind();
            SpecularTexture.ReserveMemory();

            VelocityTexture.Resize(resolution.Width, resolution.Height, 0);
            VelocityTexture.Bind();
            VelocityTexture.ReserveMemory();

            FinalTexture.Resize(resolution.Width, resolution.Height, 0);
            FinalTexture.Bind();
            FinalTexture.ReserveMemory();

            DepthStencilTexture.Resize(resolution.Width, resolution.Height, 0);
            DepthStencilTexture.Bind();
            DepthStencilTexture.ReserveMemory();
        }

        protected override void LoadTextures(Resolution resolution)
        {
            PositionTexture = new Texture(resolution.Width, resolution.Height, 0)
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

            ColorTexture = new Texture(resolution.Width, resolution.Height, 0)
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

            NormalTexture = new Texture(resolution.Width, resolution.Height, 0)
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
            NormalTexture.Bind();
            NormalTexture.ReserveMemory();

            DiffuseMaterialTexture = new Texture(resolution.Width, resolution.Height, 0)
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

            SpecularTexture = new Texture(resolution.Width, resolution.Height, 0)
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

            VelocityTexture = new Texture(resolution.Width, resolution.Height, 0)
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
            GBuffer.Add(FramebufferAttachment.ColorAttachment0, PositionTexture);
            GBuffer.Add(FramebufferAttachment.ColorAttachment1, ColorTexture);
            GBuffer.Add(FramebufferAttachment.ColorAttachment2, NormalTexture);
            GBuffer.Add(FramebufferAttachment.ColorAttachment3, DiffuseMaterialTexture);
            GBuffer.Add(FramebufferAttachment.ColorAttachment4, SpecularTexture);
            GBuffer.Add(FramebufferAttachment.ColorAttachment5, VelocityTexture);
            GBuffer.Add(FramebufferAttachment.ColorAttachment6, FinalTexture);
            GBuffer.Add(FramebufferAttachment.DepthStencilAttachment, DepthStencilTexture);

            GBuffer.Bind(FramebufferTarget.Framebuffer);
            GBuffer.AttachAttachments();
            GBuffer.Unbind(FramebufferTarget.Framebuffer);
        }

        public void GeometryPass(TextureManager textureManager, Camera camera, IEnumerable<Brush> brushes, IEnumerable<Actor> actors)
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

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);

            camera.Draw(_geometryProgram);
            _geometryProgram.SetUniform("cameraPosition", camera.Position);

            foreach (var brush in brushes)
            {
                brush.Draw(_geometryProgram, textureManager);
            }

            foreach (var actor in actors)
            {
                actor.Draw(_geometryProgram, textureManager);
            }
        }

        public void JointGeometryPass(TextureManager textureManager, Camera camera, IEnumerable<Actor> actors)
        {
            _jointGeometryProgram.Use();
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

            GL.Disable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);

            camera.Draw(_jointGeometryProgram);

            foreach (var actor in actors)
            {
                actor.Draw(_jointGeometryProgram, textureManager);
            }

            GL.Enable(EnableCap.CullFace);
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