using OpenTK.Graphics.OpenGL;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Actors;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Helpers;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Batches;
using SpiceEngineCore.Rendering.Textures;
using SweetGraphicsCore.Buffers;
using SweetGraphicsCore.Properties;
using SweetGraphicsCore.Renderers.Shaders;
using SweetGraphicsCore.Rendering.Meshes;
using SweetGraphicsCore.Rendering.Textures;
using System.Collections.Generic;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SweetGraphicsCore.Renderers.Processing
{
    public class WireframeRenderer : Renderer
    {
        private ShaderProgram _wireframeProgram;
        private ShaderProgram _jointWireframeProgram;
        private ShaderProgram _gridProgram;

        private FrameBuffer _frameBuffer = new FrameBuffer();
        private SimpleMesh _gridSquare;

        public WireframeRenderer(ITextureProvider textureProvider) : base(textureProvider) { }

        public float LineThickness { get; set; } = 0.02f;
        public Vector4 LineColor { get; set; } = Vector4.One;

        public float SelectedLineThickness { get; set; } = 0.02f;
        public Vector4 SelectedLineColor { get; set; } = new Vector4(0.7f, 0.7f, 0.1f, 1.0f);

        public float SelectedLightLineThickness { get; set; } = 0.02f;
        public Vector4 SelectedLightLineColor { get; set; } = new Vector4(0.8f, 0.8f, 0.1f, 1.0f);

        public float GridUnit { get; set; } = 1.0f;
        public float GridLength { get; set; } = 10000.0f;
        public float GridLineThickness { get; set; } = 0.02f;
        public Vector4 GridLineUnitColor { get; set; } = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);
        public Vector4 GridLineAxisColor { get; set; } = new Vector4(0.8f, 0.2f, 0.2f, 1.0f);
        public Vector4 GridLine5Color { get; set; } = new Vector4(0.4f, 0.2f, 0.2f, 1.0f);
        public Vector4 GridLine10Color { get; set; } = new Vector4(0.6f, 0.2f, 0.2f, 1.0f);
        public Quaternion GridRotation { get; set; } = Quaternion.Identity;

        public Texture FinalTexture { get; protected set; }
        public Texture DepthStencilTexture { get; protected set; }

        protected override void LoadShaders()
        {
            _wireframeProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, Resources.wireframe_vert),
                new Shader(ShaderType.GeometryShader, Resources.wireframe_geom),
                new Shader(ShaderType.FragmentShader, Resources.wireframe_frag)
            );

            _jointWireframeProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, Resources.wireframe_skinning_vert),
                new Shader(ShaderType.GeometryShader, Resources.wireframe_geom),
                new Shader(ShaderType.FragmentShader, Resources.wireframe_frag)
            );

            _gridProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, Resources.grid_vert),
                new Shader(ShaderType.FragmentShader, Resources.grid_frag)
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
            _frameBuffer.Clear();
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment0, FinalTexture);
            _frameBuffer.Add(FramebufferAttachment.DepthStencilAttachment, DepthStencilTexture);

            _frameBuffer.Bind(FramebufferTarget.Framebuffer);
            _frameBuffer.AttachAttachments();
            _frameBuffer.Unbind(FramebufferTarget.Framebuffer);

            _gridSquare = SimpleMesh.LoadFromFile(FilePathHelper.SQUARE_MESH_PATH, _gridProgram);
        }

        public void BindForWriting()
        {
            _frameBuffer.BindAndDraw(DrawBuffersEnum.ColorAttachment0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.DepthMask(false);
            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.CullFace);
        }

        /*public void VolumeWireframePass(Camera camera, IBatcher batcher)
        {
            _wireframeProgram.Use();

            camera.SetUniforms(_wireframeProgram);
            _wireframeProgram.SetUniform("lineThickness", LineThickness);
            _wireframeProgram.SetUniform("lineColor", LineColor);

            batcher.DrawVolumes(_wireframeProgram);
        }*/

        public void WireframePass(ICamera camera, IBatcher batcher)
        {
            /*_wireframeProgram.Use();
            _wireframeProgram.SetCamera(camera);
            _wireframeProgram.SetUniform("lineThickness", LineThickness);
            _wireframeProgram.SetUniform("lineColor", LineColor);
            _wireframeProgram.SetUniform("selectedLineThickness", SelectedLineThickness);
            _wireframeProgram.SetUniform("selectedLineColor", SelectedLineColor);

            foreach (var batch in batcher.GetBatches(RenderTypes.OpaqueStatic))
            {
                RenderBatch(_wireframeProgram, batcher, batch);
            }

            foreach (var batch in batcher.GetBatches(RenderTypes.TransparentStatic))
            {
                RenderBatch(_wireframeProgram, batcher, batch);
            }

            _jointWireframeProgram.Use();
            _jointWireframeProgram.SetCamera(camera);
            _jointWireframeProgram.SetUniform("lineThickness", LineThickness);
            _jointWireframeProgram.SetUniform("lineColor", LineColor);
            _jointWireframeProgram.SetUniform("selectedLineThickness", SelectedLineThickness);
            _jointWireframeProgram.SetUniform("selectedLineColor", SelectedLineColor);

            foreach (var batch in batcher.GetBatches(RenderTypes.OpaqueAnimated))
            {
                RenderBatch(_wireframeProgram, batcher, batch);
            }

            foreach (var batch in batcher.GetBatches(RenderTypes.TransparentAnimated))
            {
                RenderBatch(_wireframeProgram, batcher, batch);
            }*/

            batcher.CreateBatchAction()
                .SetShader(_wireframeProgram)
                .SetCamera(camera)
                .SetUniform("lineThickness", LineThickness)
                .SetUniform("lineColor", LineColor)
                .SetUniform("selectedLineThickness", SelectedLineThickness)
                .SetUniform("selectedLineColor", SelectedLineColor)
                .SetRenderType(RenderTypes.OpaqueStatic)
                .Render()
                .SetRenderType(RenderTypes.TransparentStatic)
                .Render()
                .SetShader(_jointWireframeProgram)
                .SetCamera(camera)
                .SetUniform("lineThickness", LineThickness)
                .SetUniform("lineColor", LineColor)
                .SetUniform("selectedLineThickness", SelectedLineThickness)
                .SetUniform("selectedLineColor", SelectedLineColor)
                .SetRenderType(RenderTypes.OpaqueAnimated)
                .Render()
                .SetRenderType(RenderTypes.TransparentAnimated)
                .Render()
                .Execute();
        }

        public void SelectionPass(ICamera camera, IEnumerable<int> entityIDs, IBatcher batcher)
        {
            /*_wireframeProgram.Use();
            _wireframeProgram.SetCamera(camera);
            _wireframeProgram.SetUniform("lineThickness", 0.0f);
            _wireframeProgram.SetUniform("lineColor", Vector4.Zero);
            _wireframeProgram.SetUniform("selectedLineThickness", SelectedLineThickness);
            _wireframeProgram.SetUniform("selectedLineColor", SelectedLineColor);

            foreach (var batch in batcher.GetBatches(RenderTypes.OpaqueStatic, entityIDs))
            {
                RenderBatch(_wireframeProgram, batcher, batch);
            }

            foreach (var batch in batcher.GetBatches(RenderTypes.TransparentStatic, entityIDs))
            {
                RenderBatch(_wireframeProgram, batcher, batch);
            }

            _jointWireframeProgram.Use();
            _jointWireframeProgram.SetCamera(camera);
            _jointWireframeProgram.SetUniform("lineThickness", 0.0f);
            _jointWireframeProgram.SetUniform("lineColor", Vector4.Zero);
            _jointWireframeProgram.SetUniform("selectedLineThickness", SelectedLineThickness);
            _jointWireframeProgram.SetUniform("selectedLineColor", SelectedLineColor);

            foreach (var batch in batcher.GetBatches(RenderTypes.OpaqueStatic, entityIDs))
            {
                RenderBatch(_jointWireframeProgram, batcher, batch);
            }

            foreach (var batch in batcher.GetBatches(RenderTypes.TransparentStatic, entityIDs))
            {
                RenderBatch(_jointWireframeProgram, batcher, batch);
            }*/

            batcher.CreateBatchAction()
                .SetEntityIDSet(entityIDs)
                .SetShader(_wireframeProgram)
                .SetCamera(camera)
                .SetUniform("lineThickness", 0.0f)
                .SetUniform("lineColor", Vector4.Zero)
                .SetUniform("selectedLineThickness", SelectedLineThickness)
                .SetUniform("selectedLineColor", SelectedLineColor)
                .SetRenderType(RenderTypes.OpaqueStatic)
                .Render()
                .SetRenderType(RenderTypes.TransparentStatic)
                .Render()
                .SetShader(_jointWireframeProgram)
                .SetCamera(camera)
                .SetUniform("lineThickness", 0.0f)
                .SetUniform("lineColor", Vector4.Zero)
                .SetUniform("selectedLineThickness", SelectedLineThickness)
                .SetUniform("selectedLineColor", SelectedLineColor)
                .SetRenderType(RenderTypes.OpaqueAnimated)
                .Render()
                .SetRenderType(RenderTypes.TransparentAnimated)
                .Render()
                .Execute();
        }

        public void RenderGridLines(ICamera camera)
        {
            _gridProgram.Use();
            _gridProgram.SetCamera(camera);

            var model = Matrix4.Identity * Matrix4.CreateFromQuaternion(GridRotation) * Matrix4.CreateScale(new Vector3(GridLength, GridLength, GridLength));
            _gridProgram.SetUniform("modelMatrix", model);

            _gridProgram.SetUniform("unit", GridUnit);
            _gridProgram.SetUniform("length", GridLength);
            _gridProgram.SetUniform("thickness", GridLineThickness);
            _gridProgram.SetUniform("lineUnitColor", GridLineUnitColor);
            _gridProgram.SetUniform("lineAxisColor", GridLineAxisColor);
            _gridProgram.SetUniform("line5Color", GridLine5Color);
            _gridProgram.SetUniform("line10Color", GridLine10Color);

            _gridSquare.Draw();
        }

        public void SelectionPass(ICamera camera, ILight light, SimpleMesh mesh)
        {
            _wireframeProgram.Use();
            _wireframeProgram.SetCamera(camera);
            _wireframeProgram.SetUniform("lineThickness", SelectedLightLineThickness);
            _wireframeProgram.SetUniform("lineColor", SelectedLightLineColor);
            _wireframeProgram.SetLight(light);

            mesh.Draw();
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
    }
}
