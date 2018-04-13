﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.IO;
using TakoEngine.Entities;
using TakoEngine.Entities.Cameras;
using TakoEngine.Entities.Lights;
using TakoEngine.Helpers;
using TakoEngine.Outputs;
using TakoEngine.Rendering.Buffers;
using TakoEngine.Rendering.Shaders;
using TakoEngine.Rendering.Textures;
using TakoEngine.Rendering.Vertices;

namespace TakoEngine.Rendering.Processing
{
    public enum TransformModes
    {
        Translate,
        Rotate,
        Scale
    }

    /// <summary>
    /// Renders all game entities to a texture, with their colors reflecting their given ID's
    /// </summary>
    public class SelectionRenderer : Renderer
    {
        public const int RED_ID = 255;
        public const int GREEN_ID = 65280;
        public const int BLUE_ID = 16711680;
        public const int CYAN_ID = 16776960;
        public const int MAGENTA_ID = 16711935;
        public const int YELLOW_ID = 65535;

        public Texture FinalTexture { get; protected set; }
        public Texture DepthStencilTexture { get; protected set; }

        public FrameBuffer GBuffer { get; private set; } = new FrameBuffer();

        private ShaderProgram _selectionProgram;
        private ShaderProgram _jointSelectionProgram;
        private ShaderProgram _translateProgram;
        private ShaderProgram _rotateProgram;
        private ShaderProgram _scaleProgram;

        private VertexArray<ColorVertex> _vertexArray = new VertexArray<ColorVertex>();
        private VertexBuffer<ColorVertex> _vertexBuffer = new VertexBuffer<ColorVertex>();

        protected override void LoadPrograms()
        {
            _selectionProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.SELECTION_VERTEX_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.SELECTION_FRAGMENT_PATH))
            );

            _jointSelectionProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.SELECTION_SKINNING_VERTEX_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.SELECTION_FRAGMENT_PATH))
            );

            _translateProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.ARROW_VERTEX_SHADER_PATH)),
                new Shader(ShaderType.GeometryShader, File.ReadAllText(FilePathHelper.ARROW_GEOMETRY_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.ARROW_FRAGMENT_SHADER_PATH))
            );

            _rotateProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.ROTATION_VERTEX_SHADER_PATH)),
                new Shader(ShaderType.GeometryShader, File.ReadAllText(FilePathHelper.ROTATION_GEOMETRY_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.ROTATION_FRAGMENT_SHADER_PATH))
            );

            _scaleProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.SCALE_VERTEX_SHADER_PATH)),
                new Shader(ShaderType.GeometryShader, File.ReadAllText(FilePathHelper.SCALE_GEOMETRY_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.SCALE_FRAGMENT_SHADER_PATH))
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
                PixelType = PixelType.UnsignedByte,
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
            _vertexBuffer.Bind();
            _vertexArray.Load(_translateProgram);
            _vertexBuffer.Unbind();

            GBuffer.Clear();
            GBuffer.Add(FramebufferAttachment.ColorAttachment0, FinalTexture);
            GBuffer.Add(FramebufferAttachment.DepthStencilAttachment, DepthStencilTexture);

            GBuffer.Bind(FramebufferTarget.Framebuffer);
            GBuffer.AttachAttachments();
            GBuffer.Unbind(FramebufferTarget.Framebuffer);
        }

        public int GetEntityIDFromPoint(Vector2 point)
        {
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, GBuffer._handle);
            GL.ReadBuffer(ReadBufferMode.ColorAttachment0);
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

            var color = FinalTexture.ReadPixelColor((int)point.X, (int)point.Y);

            return (int)(color.X + color.Y * 256 + color.Z * 256 * 256);
        }

        public void SelectionPass(Camera camera, IEnumerable<Brush> brushes, IEnumerable<Actor> actors)
        {
            _selectionProgram.Use();
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, GBuffer._handle);
            GL.DrawBuffer(DrawBufferMode.ColorAttachment0);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            camera.Draw(_selectionProgram);

            foreach (var brush in brushes)
            {
                _selectionProgram.SetUniform("id", GetColorFromID(brush.ID));
                brush.Draw(_selectionProgram);
            }

            foreach (var actor in actors)
            {
                _selectionProgram.SetUniform("id", GetColorFromID(actor.ID));
                actor.Draw(_selectionProgram);
            }
        }

        public void JointSelectionPass(Camera camera, IEnumerable<Actor> actors)
        {
            _jointSelectionProgram.Use();
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, GBuffer._handle);
            GL.DrawBuffer(DrawBufferMode.ColorAttachment0);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            camera.Draw(_jointSelectionProgram);

            foreach (var actor in actors)
            {
                _jointSelectionProgram.SetUniform("id", GetColorFromID(actor.ID));
                actor.Draw(_jointSelectionProgram);
            }
        }

        public void RenderTranslationArrows(Camera camera, Vector3 position)
        {
            _translateProgram.Use();

            camera.Draw(_translateProgram);
            _translateProgram.SetUniform("cameraPosition", camera.Position);

            _vertexBuffer.Clear();
            _vertexBuffer.AddVertex(new ColorVertex()
            {
                Position = position,
                Color = new Vector4()
            });

            _vertexArray.Bind();
            _vertexBuffer.Bind();
            _vertexBuffer.Buffer();

            GL.DrawArrays(PrimitiveType.Points, 0, _vertexBuffer.Count);

            _vertexArray.Unbind();
            _vertexBuffer.Unbind();
        }

        public void RenderRotationRings(Camera camera, Vector3 position)
        {
            _rotateProgram.Use();

            camera.Draw(_rotateProgram);
            _rotateProgram.SetUniform("cameraPosition", camera.Position);

            _vertexBuffer.Clear();
            _vertexBuffer.AddVertex(new ColorVertex()
            {
                Position = position,
                Color = new Vector4()
            });

            _vertexArray.Bind();
            _vertexBuffer.Bind();
            _vertexBuffer.Buffer();

            GL.DrawArrays(PrimitiveType.Points, 0, _vertexBuffer.Count);

            _vertexArray.Unbind();
            _vertexBuffer.Unbind();
        }

        public void RenderScaleLines(Camera camera, Vector3 position)
        {
            _scaleProgram.Use();

            camera.Draw(_scaleProgram);
            _scaleProgram.SetUniform("cameraPosition", camera.Position);

            _vertexBuffer.Clear();
            _vertexBuffer.AddVertex(new ColorVertex()
            {
                Position = position,
                Color = new Vector4()
            });

            _vertexArray.Bind();
            _vertexBuffer.Bind();
            _vertexBuffer.Buffer();

            GL.DrawArrays(PrimitiveType.Points, 0, _vertexBuffer.Count);

            _vertexArray.Unbind();
            _vertexBuffer.Unbind();
        }

        public static Vector4 GetColorFromID(int id) => new Vector4()
        {
            X = ((id & 0x000000FF) >> 0) / 255.0f,
            Y = ((id & 0x0000FF00) >> 8) / 255.0f,
            Z = ((id & 0x00FF0000) >> 16) / 255.0f,
            W = 1.0f
        };

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