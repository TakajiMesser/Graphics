using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SpiceEngineCore.Entities.Actors;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Helpers;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Batches;
using SpiceEngineCore.Rendering.Shaders;
using SweetGraphicsCore.Buffers;
using SweetGraphicsCore.Properties;
using SweetGraphicsCore.Rendering.Batches;
using SweetGraphicsCore.Rendering.Textures;
using SweetGraphicsCore.Selection;
using SweetGraphicsCore.Vertices;
using System.Collections.Generic;

namespace SweetGraphicsCore.Renderers.Processing
{
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

        private ShaderProgram _selectionProgram;
        private ShaderProgram _jointSelectionProgram;
        private ShaderProgram _translateProgram;
        private ShaderProgram _rotateProgram;
        private ShaderProgram _scaleProgram;

        private FrameBuffer _frameBuffer = new FrameBuffer();
        private VertexArray<ColorVertex3D> _vertexArray = new VertexArray<ColorVertex3D>();
        private VertexBuffer<ColorVertex3D> _vertexBuffer = new VertexBuffer<ColorVertex3D>();

        protected override void LoadPrograms()
        {
            _selectionProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, Resources.selection_vert),
                new Shader(ShaderType.FragmentShader, Resources.selection_frag)
            );

            _jointSelectionProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, Resources.selection_skinning_vert),
                new Shader(ShaderType.FragmentShader, Resources.selection_frag)
            );

            _translateProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, Resources.arrow_vert),
                new Shader(ShaderType.GeometryShader, Resources.arrow_geom),
                new Shader(ShaderType.FragmentShader, Resources.arrow_frag)
            );

            _rotateProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, Resources.rotation_vert),
                new Shader(ShaderType.GeometryShader, Resources.rotation_geom),
                new Shader(ShaderType.FragmentShader, Resources.rotation_frag)
            );

            _scaleProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, Resources.scale_vert),
                new Shader(ShaderType.GeometryShader, Resources.scale_geom),
                new Shader(ShaderType.FragmentShader, Resources.scale_frag)
            );
        }

        public override void Resize(Resolution resolution)
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
            _vertexArray.Load();
            _vertexBuffer.Unbind();

            _frameBuffer.Clear();
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment0, FinalTexture);
            _frameBuffer.Add(FramebufferAttachment.DepthStencilAttachment, DepthStencilTexture);

            _frameBuffer.Bind(FramebufferTarget.Framebuffer);
            _frameBuffer.AttachAttachments();
            _frameBuffer.Unbind(FramebufferTarget.Framebuffer);
        }

        public void BindForReading()
        {
            _frameBuffer.BindAndRead(ReadBufferMode.ColorAttachment0);
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
        }

        public void BindForWriting()
        {
            _frameBuffer.BindAndDraw(DrawBuffersEnum.ColorAttachment0);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
        }

        public int GetEntityIDFromPoint(Vector2 point)
        {
            var color = FinalTexture.ReadPixelColor((int)point.X, (int)point.Y);
            return SelectionHelper.GetIDFromColorVector(color);
        }

        // IEntityProvider entityProvider, Camera camera, BatchManager batchManager, TextureManager textureManager
        public void SelectionPass(ICamera camera, IBatcher batcher, IEnumerable<int> ids)
        {
            batcher.CreateBatchAction()
                .SetShader(_selectionProgram)
                .SetCamera(camera)
                .SetEntityIDSet(ids)
                .SetRenderType(RenderTypes.OpaqueStatic)
                .Render()
                .SetRenderType(RenderTypes.TransparentStatic)
                .Render()
                //.RenderOpaqueStaticWithAction(id => _selectionProgram.SetUniform("id", GetColorFromID(id)))
                //.RenderTransparentStaticWithAction(id => _selectionProgram.SetUniform("id", GetColorFromID(id)))
                .SetShader(_jointSelectionProgram)
                .SetCamera(camera)
                .SetRenderType(RenderTypes.OpaqueAnimated)
                .Render()
                .SetRenderType(RenderTypes.TransparentAnimated)
                .Render()
                //.RenderOpaqueAnimatedWithAction(id => _jointSelectionProgram.SetUniform("id", GetColorFromID(id)))
                //.RenderTransparentAnimatedWithAction(id => _jointSelectionProgram.SetUniform("id", GetColorFromID(id)))
                .Execute();
        }

        //public void UIPass(ICamera camera, IBatcher batcher, IEntityProvider entity)
        
        public void RenderTranslationArrows(ICamera camera, Vector3 position) { RenderTranslationArrows(camera, position, Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ); }
        public void RenderTranslationArrows(ICamera camera, Vector3 position, Vector3 xDirection, Vector3 yDirection, Vector3 zDirection)
        {
            _translateProgram.Use();

            camera.SetUniforms(_translateProgram);
            _translateProgram.SetUniform("cameraPosition", camera.Position);
            _translateProgram.SetUniform("xDirection", xDirection);
            _translateProgram.SetUniform("yDirection", yDirection);
            _translateProgram.SetUniform("zDirection", zDirection);

            _vertexBuffer.Clear();
            _vertexBuffer.AddVertex(new ColorVertex3D(position, new Color4()));

            _vertexArray.Bind();
            _vertexBuffer.Bind();
            _vertexBuffer.Buffer();

            GL.DrawArrays(PrimitiveType.Points, 0, _vertexBuffer.Count);

            _vertexArray.Unbind();
            _vertexBuffer.Unbind();
        }

        public void RenderRotationRings(ICamera camera, Vector3 position)
        {
            _rotateProgram.Use();

            camera.SetUniforms(_rotateProgram);
            _rotateProgram.SetUniform("cameraPosition", camera.Position);

            _vertexBuffer.Clear();
            _vertexBuffer.AddVertex(new ColorVertex3D(position, new Color4()));

            _vertexArray.Bind();
            _vertexBuffer.Bind();
            _vertexBuffer.Buffer();

            GL.DrawArrays(PrimitiveType.Points, 0, _vertexBuffer.Count);

            _vertexArray.Unbind();
            _vertexBuffer.Unbind();
        }

        public void RenderScaleLines(ICamera camera, Vector3 position)
        {
            _scaleProgram.Use();

            camera.SetUniforms(_scaleProgram);
            _scaleProgram.SetUniform("cameraPosition", camera.Position);

            _vertexBuffer.Clear();
            _vertexBuffer.AddVertex(new ColorVertex3D(position, new Color4()));

            _vertexArray.Bind();
            _vertexBuffer.Bind();
            _vertexBuffer.Buffer();

            GL.DrawArrays(PrimitiveType.Points, 0, _vertexBuffer.Count);

            _vertexArray.Unbind();
            _vertexBuffer.Unbind();
        }

        private IEnumerable<Actor> PerformFrustumCulling(IEnumerable<Actor> actors)
        {
            // Don't render meshes that are not in the camera's view

            // Using the position of the actor, determine if we should render the mesh
            // We will also need a bounding sphere or bounding box from the mesh to determine this
            foreach (var actor in actors)
            {
                Vector3 position = actor.Position;
            }

            return actors;
        }

        private IEnumerable<Actor> PerformOcclusionCulling(IEnumerable<Actor> actors)
        {
            // Don't render meshes that are obscured by closer meshes
            foreach (var actor in actors)
            {
                Vector3 position = actor.Position;
            }

            return actors;
        }

        public static bool IsReservedID(int id) => id == RED_ID || id == GREEN_ID || id == BLUE_ID || id == CYAN_ID || id == MAGENTA_ID || id == YELLOW_ID;

        public static SelectionTypes GetSelectionTypeFromID(int id)
        {
            switch (id)
            {
                case RED_ID:
                    return SelectionTypes.Red;
                case GREEN_ID:
                    return SelectionTypes.Green;
                case BLUE_ID:
                    return SelectionTypes.Blue;
                case CYAN_ID:
                    return SelectionTypes.Cyan;
                case MAGENTA_ID:
                    return SelectionTypes.Magenta;
                case YELLOW_ID:
                    return SelectionTypes.Yellow;
                default:
                    return SelectionTypes.None;
            }
        }
    }
}
