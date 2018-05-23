using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TakoEngine.Entities;
using TakoEngine.Entities.Cameras;
using TakoEngine.Entities.Lights;
using TakoEngine.Helpers;
using TakoEngine.Outputs;
using TakoEngine.Rendering.Buffers;
using TakoEngine.Rendering.Shaders;
using TakoEngine.Rendering.Textures;
using TakoEngine.Rendering.Vertices;
using TakoEngine.Utilities;

namespace TakoEngine.Rendering.Processing
{
    /// <summary>
    /// Renders entities as billboards (quads that always face the camera)
    /// </summary>
    public class BillboardRenderer : Renderer
    {
        public Texture FinalTexture { get; protected set; }

        private ShaderProgram _billboardProgram;
        private ShaderProgram _billboardSelectionProgram;

        private VertexArray<ColorVertex> _vertexArray = new VertexArray<ColorVertex>();
        private VertexBuffer<ColorVertex> _vertexBuffer = new VertexBuffer<ColorVertex>();

        private Texture _pointLightTexture;
        private Texture _spotLightTexture;
        private Texture _directionalLightTexture;

        private Texture _selectedPointLightTexture;
        private Texture _selectedSpotLightTexture;
        private Texture _selectedDirectionalLightTexture;

        protected override void LoadPrograms()
        {
            _billboardProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.BILLBOARD_VERTEX_SHADER_PATH)),
                new Shader(ShaderType.GeometryShader, File.ReadAllText(FilePathHelper.BILLBOARD_GEOMETRY_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.BILLBOARD_FRAGMENT_SHADER_PATH))
            );

            _billboardSelectionProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.BILLBOARD_SELECTION_VERTEX_SHADER_PATH)),
                new Shader(ShaderType.GeometryShader, File.ReadAllText(FilePathHelper.BILLBOARD_SELECTION_GEOMETRY_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.BILLBOARD_SELECTION_FRAGMENT_SHADER_PATH))
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

            _pointLightTexture = Texture.LoadFromFile(FilePathHelper.POINT_LIGHT_BILLBOARD_TEXTURE_PATH, false, false);
            _spotLightTexture = Texture.LoadFromFile(FilePathHelper.SPOT_LIGHT_BILLBOARD_TEXTURE_PATH, false, false);
            _directionalLightTexture = Texture.LoadFromFile(FilePathHelper.DIRECTIONAL_LIGHT_BILLBOARD_TEXTURE_PATH, false, false);

            _selectedPointLightTexture = Texture.LoadFromFile(FilePathHelper.SELECTED_POINT_LIGHT_BILLBOARD_TEXTURE_PATH, false, false);
            _selectedSpotLightTexture = Texture.LoadFromFile(FilePathHelper.SELECTED_SPOT_LIGHT_BILLBOARD_TEXTURE_PATH, false, false);
            _selectedDirectionalLightTexture = Texture.LoadFromFile(FilePathHelper.SELECTED_DIRECTIONAL_LIGHT_BILLBOARD_TEXTURE_PATH, false, false);
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
                _vertexBuffer.AddVertex(new ColorVertex()
                {
                    Position = entity.Position,
                    Color = SelectionRenderer.GetColorFromID(entity.ID)
                });
            }

            _vertexArray.Bind();
            _vertexBuffer.Bind();
            _vertexBuffer.Buffer();

            GL.DrawArrays(PrimitiveType.Points, 0, _vertexBuffer.Count);

            _vertexArray.Unbind();
            _vertexBuffer.Unbind();
        }

        public void RenderLights(Camera camera, IEnumerable<Light> lights)
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

        public void RenderSelection(Camera camera, Volume volume)
        {
            _billboardProgram.Use();

            camera.SetUniforms(_billboardProgram);
            _billboardProgram.SetUniform("cameraPosition", camera.Position);

            // Need to bind a texture for each selectable vertex point
            _billboardProgram.BindTexture(_selectedPointLightTexture, "mainTexture", 0);

            _vertexBuffer.Clear();

            foreach (var vertex in volume.Mesh.Vertices)
            {
                _vertexBuffer.AddVertex(new ColorVertex()
                {
                    Position = vertex.Position,
                    Color = new Vector4(vertex.Color.X * 1.5f, vertex.Color.Y * 1.5f, vertex.Color.Z * 1.5f, 1.0f)
                });
            }

            _vertexArray.Bind();
            _vertexBuffer.Bind();
            _vertexBuffer.Buffer();

            GL.DrawArrays(PrimitiveType.Points, 0, _vertexBuffer.Count);

            _vertexArray.Unbind();
            _vertexBuffer.Unbind();
        }

        public void RenderSelection(Camera camera, Light light)
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

        public void RenderLightSelections(Camera camera, IEnumerable<Light> lights)
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

        private void DrawLights(IEnumerable<Light> lights)
        {
            _vertexBuffer.Clear();
            foreach (var light in lights)
            {
                _vertexBuffer.AddVertex(new ColorVertex()
                {
                    Position = light.Position,
                    Color = SelectionRenderer.GetColorFromID(light.ID)
                });
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
