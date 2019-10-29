using OpenTK.Graphics.OpenGL;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Helpers;
using SpiceEngineCore.Rendering.Buffers;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Shaders;
using SpiceEngineCore.Rendering.Textures;
using SpiceEngineCore.Rendering.Vertices;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngineCore.Rendering.Batches
{
    public class BillboardBatch : Batch
    {
        private TextureID _textureID;
        private VertexArray<ColorVertex3D> _vertexArray = new VertexArray<ColorVertex3D>();
        private VertexBuffer<ColorVertex3D> _vertexBuffer = new VertexBuffer<ColorVertex3D>();

        private List<ColorVertex3D> _vertices = new List<ColorVertex3D>();
        private float _alpha = 1.0f;

        private Dictionary<int, int> _indexByID = new Dictionary<int, int>();

        public bool IsTransparent => _alpha < 1.0f;

        public BillboardBatch(TextureID textureID) => _textureID = textureID;

        public override IBatch Duplicate() => new BillboardBatch(_textureID);

        public override void AddEntity(int id, IRenderable renderable)
        {
            if (renderable is TextureID textureID)
            {
                var index = EntityIDs.Count();
                _indexByID.Add(id, index);

                _vertices.Add(new ColorVertex3D(textureID.Position, SelectionHelper.GetColorFromID(id)));
                _vertexBuffer.AddVertex(_vertices[index]);
            }

            base.AddEntity(id, renderable);
        }

        public override void Transform(int entityID, Transform transform)
        {
            var index = _indexByID[entityID];

            var transformedVertex = (ColorVertex3D)_vertices[index].Transformed(transform);
            _vertices[index] = transformedVertex;

            // TODO - This is very redundant to keep two separate lists of vertex (struct) data
            if (_vertexBuffer != null)
            {
                _vertexBuffer.Clear();
                _vertexBuffer.AddVertices(_vertices);
            }
        }

        public override void Load()
        {
            _vertexBuffer.Bind();
            _vertexArray.Load();
            _vertexBuffer.Unbind();

            _vertexBuffer.AddVertices(_vertices);

            //_texture = Texture.Load(Resources.vertex, false, false);
            base.Load();
        }

        public override void Draw(IEntityProvider entityProvider, ShaderProgram shaderProgram, ITextureProvider textureProvider = null)
        {
            if (textureProvider != null)
            {
                // TODO - Also set texture Alpha value here
                var texture = textureProvider.RetrieveTexture(_textureID.Index);
                shaderProgram.BindTexture(texture, "mainTexture", 0);
            }
            
            /*_vertexBuffer.Clear();
            foreach (var id in EntityIDs)
            {
                var entity = entityProvider.GetEntity(id);
                _vertexBuffer.AddVertex(new ColorVertex3D(entity.Position, SelectionRenderer.GetColorFromID(id)));
            }*/

            _vertexArray.Bind();
            _vertexBuffer.Bind();
            _vertexBuffer.Buffer();

            GL.DrawArrays(PrimitiveType.Points, 0, _vertexBuffer.Count);

            _vertexArray.Unbind();
            _vertexBuffer.Unbind();
        }
    }
}
