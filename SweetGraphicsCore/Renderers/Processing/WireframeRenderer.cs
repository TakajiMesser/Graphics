using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Actors;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Helpers;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Batches;
using SweetGraphicsCore.Buffers;
using SweetGraphicsCore.Helpers;
using SweetGraphicsCore.Properties;
using SweetGraphicsCore.Rendering.Batches;
using SweetGraphicsCore.Rendering.Meshes;
using SweetGraphicsCore.Rendering.Textures;
using SweetGraphicsCore.Shaders;
using System.Collections.Generic;

namespace SweetGraphicsCore.Renderers.Processing
{
    public class WireframeRenderer : Renderer
    {
        private ShaderProgram _wireframeProgram;
        private ShaderProgram _jointWireframeProgram;
        private ShaderProgram _gridProgram;

        private FrameBuffer _frameBuffer;
        private SimpleMesh _gridSquare;

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

        protected override void LoadPrograms(IRenderContext renderContext)
        {
            _wireframeProgram = ShaderHelper.LoadProgram(renderContext,
                new[] { ShaderType.VertexShader, ShaderType.GeometryShader, ShaderType.FragmentShader },
                new[] { Resources.wireframe_vert, Resources.wireframe_geom, Resources.wireframe_frag });

            _jointWireframeProgram = ShaderHelper.LoadProgram(renderContext,
                new[] { ShaderType.VertexShader, ShaderType.GeometryShader, ShaderType.FragmentShader },
                new[] { Resources.wireframe_skinning_vert, Resources.wireframe_geom, Resources.wireframe_frag });

            _gridProgram = ShaderHelper.LoadProgram(renderContext,
                new[] { ShaderType.VertexShader, ShaderType.FragmentShader },
                new[] { Resources.grid_vert, Resources.grid_frag });
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
                PixelType = PixelType.Float,
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
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment0, FinalTexture);
            _frameBuffer.Add(FramebufferAttachment.StencilAttachment, DepthStencilTexture);
            _frameBuffer.Load();

            _gridSquare = SimpleMesh.LoadFromFile(renderContext, FilePathHelper.SQUARE_MESH_PATH, _gridProgram);
        }

        protected override void Resize(Resolution resolution)
        {
            FinalTexture.Resize(resolution.Width, resolution.Height, 0);
            DepthStencilTexture.Resize(resolution.Width, resolution.Height, 0);
        }

        public void BindForWriting()
        {
            _frameBuffer.BindAndDraw(DrawBufferMode.ColorAttachment0);
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
            _gridProgram.Bind();
            _gridProgram.SetCamera(camera);

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

        // TODO - Pass multiple entity ID's to this method to render all selections at once
        public void SelectionPass(IEntityProvider entityProvider, ICamera camera, IEntity entity, IBatcher batcher)
        {
            /*batcher.CreateBatchAction()
                .SetShader(_wireframeProgram)
                .SetCamera(camera)
                .SetEntityIDSet(entity.ID.Yield())
                .SetUniform("lineThickness", SelectedLightLineThickness)
                .SetUniform("lineColor", SelectedLineColor)
                .SetRenderType(RenderTypes.OpaqueStatic)
                .Render()
                .SetShader(_jointWireframeProgram)
                .SetCamera(camera)
                .SetEntityIDSet(entity.ID.Yield())
                .SetUniform("lineThickness", SelectedLightLineThickness)
                .SetUniform("lineColor", SelectedLineColor)
                .SetRenderType(RenderTypes.OpaqueAnimated)
                .Render()
                .Execute();*/

            /*var batch = batchManager.GetBatch(entity.ID);

            //var program = entity is AnimatedActor ? _jointWireframeProgram : _wireframeProgram;
            var program = batch is ModelBatch modelBatch && modelBatch.Model is IAnimatedModel animatedModel
                ? _jointWireframeProgram
                : _wireframeProgram;

            program.Use();

            camera.SetUniforms(program);
            program.SetUniform("lineThickness", SelectedLineThickness);
            program.SetUniform("lineColor", SelectedLineColor);
            
            batch.Draw(entityProvider, program);*/
        }

        public void SelectionPass(ICamera camera, ILight light, SimpleMesh mesh)
        {
            _wireframeProgram.Bind();
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
