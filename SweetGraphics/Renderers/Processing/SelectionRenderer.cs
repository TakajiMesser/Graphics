using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngineCore.Entities.Actors;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Helpers;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Batches;
using SweetGraphicsCore.Buffers;
using SweetGraphicsCore.Helpers;
using SweetGraphics.Properties;
using SweetGraphicsCore.Rendering.Batches;
using SweetGraphicsCore.Rendering.Textures;
using SweetGraphicsCore.Selection;
using SweetGraphicsCore.Shaders;
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

        private FrameBuffer _frameBuffer;
        private VertexArray<ColorVertex3D> _vertexArray;
        private VertexBuffer<ColorVertex3D> _vertexBuffer;

        private static object _dumbassLock = new object();
        private static int _dumbassCounter = 0;
        private int _dumbassID = 0;

        public SelectionRenderer()
        {
            lock (_dumbassLock)
            {
                _dumbassCounter++;
                _dumbassID = _dumbassCounter;
            }
        }

        protected override void LoadPrograms(IRenderContext renderContext)
        {
            _selectionProgram = ShaderHelper.LoadProgram(renderContext,
                new[] { ShaderType.VertexShader, ShaderType.FragmentShader },
                new[] { Resources.selection_vert, Resources.selection_frag });

            _jointSelectionProgram = ShaderHelper.LoadProgram(renderContext,
                new[] { ShaderType.VertexShader, ShaderType.FragmentShader },
                new[] { Resources.selection_skinning_vert, Resources.selection_frag });

            _translateProgram = ShaderHelper.LoadProgram(renderContext,
                new[] { ShaderType.VertexShader, ShaderType.GeometryShader, ShaderType.FragmentShader },
                new[] { Resources.arrow_vert, Resources.arrow_geom, Resources.arrow_frag });

            _rotateProgram = ShaderHelper.LoadProgram(renderContext,
                new[] { ShaderType.VertexShader, ShaderType.GeometryShader, ShaderType.FragmentShader },
                new[] { Resources.rotation_vert, Resources.rotation_geom, Resources.rotation_frag });

            _scaleProgram = ShaderHelper.LoadProgram(renderContext,
                new[] { ShaderType.VertexShader, ShaderType.GeometryShader, ShaderType.FragmentShader },
                new[] { Resources.scale_vert, Resources.scale_geom, Resources.scale_frag });
        }

        protected override void LoadTextures(IRenderContext renderContext, Resolution resolution)
        {
            FinalTexture = new Texture(renderContext, resolution.Width, resolution.Height, 0)
            {
                Target = TextureTarget.Texture2d,
                EnableMipMap = false,
                EnableAnisotropy = false,
                InternalFormat = InternalFormat.Rgba16f,
                PixelFormat = PixelFormat.Rgba,
                PixelType = PixelType.UnsignedByte,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            FinalTexture.Load();

            DepthStencilTexture = new Texture(renderContext, resolution.Width, resolution.Height, 0)
            {
                Target = TextureTarget.Texture2d,
                EnableMipMap = false,
                EnableAnisotropy = false,
                InternalFormat = InternalFormat.Depth32fStencil8,
                PixelFormat = PixelFormat.DepthComponent,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            DepthStencilTexture.Load();
        }

        protected override void LoadBuffers(IRenderContext renderContext)
        {
            _frameBuffer = new FrameBuffer(renderContext);
            _vertexArray = new VertexArray<ColorVertex3D>(renderContext);
            _vertexBuffer = new VertexBuffer<ColorVertex3D>(renderContext);

            _vertexBuffer.Load();
            _vertexBuffer.Bind();
            _vertexArray.Load();
            _vertexBuffer.Unbind();

            _frameBuffer.Add(FramebufferAttachment.ColorAttachment0, FinalTexture);
            _frameBuffer.Add(FramebufferAttachment.StencilAttachment, DepthStencilTexture);
            _frameBuffer.Load();
        }

        protected override void Resize(Resolution resolution)
        {
            FinalTexture.Resize(resolution.Width, resolution.Height, 0);
            DepthStencilTexture.Resize(resolution.Width, resolution.Height, 0);
        }

        public void BindForReading()
        {
            _frameBuffer.BindAndRead(ReadBufferMode.ColorAttachment0);
            GL.PixelStorei(PixelStoreParameter.UnpackAlignment, 1);
        }

        public void BindForWriting()
        {
            _frameBuffer.BindAndDraw(DrawBufferMode.ColorAttachment0);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
        }

        public int GetEntityIDFromPoint(Vector2 point)
        {
            var color = FinalTexture.ReadPixelColor((int)point.X, (int)point.Y);
            return SelectionHelper.GetIDFromColor(color);
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
            _translateProgram.Bind();

            _translateProgram.SetCamera(camera);
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
            _rotateProgram.Bind();

            _rotateProgram.SetCamera(camera);
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
            _scaleProgram.Bind();

            _scaleProgram.SetCamera(camera);
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
