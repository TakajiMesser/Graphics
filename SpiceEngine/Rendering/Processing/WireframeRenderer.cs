using OpenTK;
using OpenTK.Graphics.OpenGL;
using SpiceEngine.Entities;
using SpiceEngine.Entities.Actors;
using SpiceEngine.Entities.Cameras;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Helpers;
using SpiceEngine.Outputs;
using SpiceEngine.Properties;
using SpiceEngine.Rendering.Batches;
using SpiceEngine.Rendering.Buffers;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using System.Collections.Generic;

namespace SpiceEngine.Rendering.Processing
{
    public class WireframeRenderer : Renderer
    {
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

        private ShaderProgram _wireframeProgram;
        private ShaderProgram _jointWireframeProgram;
        private ShaderProgram _gridProgram;

        private FrameBuffer _frameBuffer = new FrameBuffer();
        private SimpleMesh _gridSquare;

        protected override void LoadPrograms()
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

        /*public void VolumeWireframePass(Camera camera, BatchManager batchManager)
        {
            _wireframeProgram.Use();

            camera.SetUniforms(_wireframeProgram);
            _wireframeProgram.SetUniform("lineThickness", LineThickness);
            _wireframeProgram.SetUniform("lineColor", LineColor);

            batchManager.DrawVolumes(_wireframeProgram);
        }*/

        public void WireframePass(ICamera camera, BatchManager batchManager)
        {
            batchManager.CreateBatchAction()
                .SetShader(_wireframeProgram)
                .SetCamera(camera)
                .SetUniform("lineThickness", LineThickness)
                .SetUniform("lineColor", LineColor)
                .SetUniform("selectedLineThickness", SelectedLineThickness)
                .SetUniform("selectedLineColor", SelectedLineColor)
                .RenderOpaqueStatic()
                .RenderTransparentStatic()
                .SetShader(_jointWireframeProgram)
                .SetCamera(camera)
                .SetUniform("lineThickness", LineThickness)
                .SetUniform("lineColor", LineColor)
                .SetUniform("selectedLineThickness", SelectedLineThickness)
                .SetUniform("selectedLineColor", SelectedLineColor)
                .RenderOpaqueAnimated()
                .RenderTransparentAnimated()
                .Execute();
        }

        public void SelectionPass(ICamera camera, IEnumerable<int> entityIDs, BatchManager batchManager)
        {
            batchManager.CreateBatchAction()
                .SetEntityIDs(entityIDs)
                .SetShader(_wireframeProgram)
                .SetCamera(camera)
                .SetUniform("lineThickness", 0.0f)
                .SetUniform("lineColor", Vector4.Zero)
                .SetUniform("selectedLineThickness", SelectedLineThickness)
                .SetUniform("selectedLineColor", SelectedLineColor)
                .RenderOpaqueStatic()
                .RenderTransparentStatic()
                .SetShader(_jointWireframeProgram)
                .SetCamera(camera)
                .SetUniform("lineThickness", 0.0f)
                .SetUniform("lineColor", Vector4.Zero)
                .SetUniform("selectedLineThickness", SelectedLineThickness)
                .SetUniform("selectedLineColor", SelectedLineColor)
                .RenderOpaqueAnimated()
                .RenderTransparentAnimated()
                .Execute();
        }

        public void RenderGridLines(ICamera camera)
        {
            _gridProgram.Use();

            camera.SetUniforms(_gridProgram);

            var model = Matrix4.Identity * Matrix4.CreateFromQuaternion(GridRotation) * Matrix4.CreateScale(GridLength);
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

        public void SelectionPass(IEntityProvider entityProvider, ICamera camera, IEntity entity, BatchManager batchManager)
        {
            var program = entity is AnimatedActor ? _jointWireframeProgram : _wireframeProgram;
            program.Use();

            camera.SetUniforms(program);
            program.SetUniform("lineThickness", SelectedLineThickness);
            program.SetUniform("lineColor", SelectedLineColor);

            var batch = batchManager.GetBatch(entity.ID);
            batch.Draw(entityProvider, program);
        }

        public void SelectionPass(ICamera camera, ILight light, SimpleMesh mesh)
        {
            _wireframeProgram.Use();

            camera.SetUniforms(_wireframeProgram);
            _wireframeProgram.SetUniform("lineThickness", SelectedLightLineThickness);
            _wireframeProgram.SetUniform("lineColor", SelectedLightLineColor);

            light.SetUniforms(_wireframeProgram);

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
