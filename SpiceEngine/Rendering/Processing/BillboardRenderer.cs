using OpenTK;
using OpenTK.Graphics.OpenGL;
using SpiceEngine.Entities;
using SpiceEngine.Entities.Actors;
using SpiceEngine.Entities.Cameras;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Entities.Volumes;
using SpiceEngine.Outputs;
using SpiceEngine.Properties;
using SpiceEngine.Rendering.Batches;
using SpiceEngine.Rendering.Buffers;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;
using SpiceEngine.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Rendering.Processing
{
    /// <summary>
    /// Renders entities as billboards (quads that always face the camera)
    /// </summary>
    public class BillboardRenderer : Renderer
    {
        public Texture FinalTexture { get; protected set; }

        private ShaderProgram _billboardProgram;
        private ShaderProgram _billboardSelectionProgram;

        private VertexArray<ColorVertex3D> _vertexArray = new VertexArray<ColorVertex3D>();
        private VertexBuffer<ColorVertex3D> _vertexBuffer = new VertexBuffer<ColorVertex3D>();

        private Texture _vertexTexture;

        private Texture _pointLightTexture;
        private Texture _spotLightTexture;
        private Texture _directionalLightTexture;

        private Texture _selectedPointLightTexture;
        private Texture _selectedSpotLightTexture;
        private Texture _selectedDirectionalLightTexture;

        protected override void LoadPrograms()
        {
            _billboardProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, Resources.billboard_vert),
                new Shader(ShaderType.GeometryShader, Resources.billboard_geom),
                new Shader(ShaderType.FragmentShader, Resources.billboard_frag)
            );

            _billboardSelectionProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, Resources.billboard_selection_vert),
                new Shader(ShaderType.GeometryShader, Resources.billboard_selection_geom),
                new Shader(ShaderType.FragmentShader, Resources.billboard_selection_frag)
            );
        }

        public override void ResizeTextures(Resolution resolution)
        {
            FinalTexture.Resize(resolution.Width, resolution.Height, 0);
            FinalTexture.Bind();
            FinalTexture.ReserveMemory();
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
        }

        protected override void LoadBuffers()
        {
            _vertexBuffer.Bind();
            _vertexArray.Load();
            _vertexBuffer.Unbind();

            _vertexTexture = Texture.Load(Resources.vertex, false, false);

            _pointLightTexture = Texture.Load(Resources.point_light_billboard, false, false);
            _spotLightTexture = Texture.Load(Resources.spot_light_billboard, false, false);
            _directionalLightTexture = Texture.Load(Resources.directional_light_billboard, false, false);

            _selectedPointLightTexture = Texture.Load(Resources.selected_point_light, false, false);
            _selectedSpotLightTexture = Texture.Load(Resources.selected_spot_light, false, false);
            _selectedDirectionalLightTexture = Texture.Load(Resources.selected_directional_light, false, false);
        }

        public void RenderEntities(Camera camera, IEnumerable<IEntity> entities, Texture texture)
        {
            _billboardProgram.Use();
            _billboardProgram.BindTexture(texture, "mainTexture", 0);

            camera.SetUniforms(_billboardProgram);
            _billboardProgram.SetUniform("cameraPosition", camera.Position);

            _vertexBuffer.Clear();
            foreach (var entity in entities)
            {
                _vertexBuffer.AddVertex(new ColorVertex3D(entity.Position, SelectionRenderer.GetColorFromID(entity.ID)));
            }

            _vertexArray.Bind();
            _vertexBuffer.Bind();
            _vertexBuffer.Buffer();

            GL.DrawArrays(PrimitiveType.Points, 0, _vertexBuffer.Count);

            _vertexArray.Unbind();
            _vertexBuffer.Unbind();
        }

        public void RenderLights(Camera camera, IEnumerable<ILight> lights)
        {
            _billboardProgram.Use();

            camera.SetUniforms(_billboardProgram);
            _billboardProgram.SetUniform("cameraPosition", camera.Position);

            _billboardProgram.BindTexture(_pointLightTexture, "mainTexture", 0);
            DrawLights(lights.Where(l => l is PointLight));

            _billboardProgram.BindTexture(_spotLightTexture, "mainTexture", 0);
            DrawLights(lights.Where(l => l is SpotLight));

            _billboardProgram.BindTexture(_directionalLightTexture, "mainTexture", 0);
            DrawLights(lights.Where(l => l is DirectionalLight));
        }

        public void RenderSelection(Camera camera, Volume volume, BatchManager batchManager)
        {
            _billboardProgram.Use();

            camera.SetUniforms(_billboardProgram);
            _billboardProgram.SetUniform("cameraPosition", camera.Position);

            // Need to bind a texture for each selectable vertex point
            _billboardProgram.BindTexture(_vertexTexture, "mainTexture", 0);

            _vertexBuffer.Clear();

            var batch = batchManager.GetBatch(volume.ID);
            foreach (var vertex in batch.Vertices)
            {
                //_vertexBuffer.AddVertex(new ColorVertex3D(volume.Position + vertex.Position, new Vector4(vertex.Color.X * 1.5f, vertex.Color.Y * 1.5f, vertex.Color.Z * 1.5f, 1.0f)));
            }

            _vertexArray.Bind();
            _vertexBuffer.Bind();
            _vertexBuffer.Buffer();

            GL.DrawArrays(PrimitiveType.Points, 0, _vertexBuffer.Count);

            _vertexArray.Unbind();
            _vertexBuffer.Unbind();
        }

        public void RenderSelection(Camera camera, ILight light)
        {
            _billboardProgram.Use();

            camera.SetUniforms(_billboardProgram);
            _billboardProgram.SetUniform("cameraPosition", camera.Position);

            switch (light)
            {
                case PointLight p:
                    _billboardProgram.BindTexture(_selectedPointLightTexture, "mainTexture", 0);
                    break;
                case SpotLight s:
                    _billboardProgram.BindTexture(_selectedSpotLightTexture, "mainTexture", 0);
                    break;
                case DirectionalLight d:
                    _billboardProgram.BindTexture(_selectedDirectionalLightTexture, "mainTexture", 0);
                    break;
            }

            DrawLights(light.Yield());
        }

        public void RenderVertexSelection(Camera camera, IEntity entity)
        {
            _billboardProgram.Use();

            camera.SetUniforms(_billboardProgram);
            _billboardProgram.SetUniform("cameraPosition", camera.Position);

            _billboardProgram.BindTexture(_vertexTexture, "mainTexture", 0);
            DrawVertices(entities.Yield());
        }

        public void RenderVertexSelectIDs(Camera camera, IEnumerable<IEntity> entities)
        {
            _billboardSelectionProgram.Use();

            camera.SetUniforms(_billboardSelectionProgram);
            _billboardSelectionProgram.SetUniform("cameraPosition", camera.Position);

            _billboardSelectionProgram.BindTexture(_vertexTexture, "mainTexture", 0);
            DrawVertices(entities);
        }

        public void RenderLightSelectIDs(Camera camera, IEnumerable<ILight> lights)
        {
            _billboardSelectionProgram.Use();

            camera.SetUniforms(_billboardSelectionProgram);
            _billboardSelectionProgram.SetUniform("cameraPosition", camera.Position);

            _billboardSelectionProgram.BindTexture(_pointLightTexture, "mainTexture", 0);
            DrawLights(lights.Where(l => l is PointLight));

            _billboardSelectionProgram.BindTexture(_spotLightTexture, "mainTexture", 0);
            DrawLights(lights.Where(l => l is SpotLight));

            _billboardSelectionProgram.BindTexture(_directionalLightTexture, "mainTexture", 0);
            DrawLights(lights.Where(l => l is DirectionalLight));
        }

        private void DrawVertices(IEnumerable<IEntity> entities)
        {
            _vertexBuffer.Clear();
            foreach (var entity in entities)
            {
                _vertexBuffer.AddVertex(new ColorVertex3D(entity.Position, SelectionRenderer.GetColorFromID(entity.ID)));
            }

            _vertexArray.Bind();
            _vertexBuffer.Bind();
            _vertexBuffer.Buffer();

            GL.DrawArrays(PrimitiveType.Points, 0, _vertexBuffer.Count);

            _vertexArray.Unbind();
            _vertexBuffer.Unbind();
        }

        private void DrawLights(IEnumerable<ILight> lights)
        {
            _vertexBuffer.Clear();
            foreach (var light in lights)
            {
                _vertexBuffer.AddVertex(new ColorVertex3D(light.Position, SelectionRenderer.GetColorFromID(light.ID)));
            }

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
    }
}
