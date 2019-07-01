using OpenTK.Graphics.OpenGL;
using SpiceEngine.Entities;
using SpiceEngine.Rendering.Buffers;
using SpiceEngine.Rendering.Processing;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;

namespace SpiceEngine.Rendering.Batches
{
    public class BillboardBatch : Batch
    {
        private TextureID _textureID;
        private VertexArray<ColorVertex3D> _vertexArray = new VertexArray<ColorVertex3D>();
        private VertexBuffer<ColorVertex3D> _vertexBuffer = new VertexBuffer<ColorVertex3D>();

        public BillboardBatch(TextureID textureID) => _textureID = textureID;

        public override IBatch Duplicate() => new BillboardBatch(_textureID);

        public override void Load()
        {
            _vertexBuffer.Bind();
            _vertexArray.Load();
            _vertexBuffer.Unbind();

            //_texture = Texture.Load(Resources.vertex, false, false);
        }

        public override void Draw(IEntityProvider entityProvider, ShaderProgram shaderProgram, ITextureProvider textureProvider = null)
        {
            if (textureProvider != null)
            {
                // TODO - Also set texture Alpha value here
                var texture = textureProvider.RetrieveTexture(_textureID.ID);
                shaderProgram.BindTexture(texture, "mainTexture", 0);
            }
            
            _vertexBuffer.Clear();
            foreach (var id in EntityIDs)
            {
                var entity = entityProvider.GetEntity(id);
                _vertexBuffer.AddVertex(new ColorVertex3D(entity.Position, SelectionRenderer.GetColorFromID(id)));
            }

            _vertexArray.Bind();
            _vertexBuffer.Bind();
            _vertexBuffer.Buffer();

            GL.DrawArrays(PrimitiveType.Points, 0, _vertexBuffer.Count);

            _vertexArray.Unbind();
            _vertexBuffer.Unbind();
        }
    }
}
