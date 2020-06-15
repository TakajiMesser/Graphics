using SpiceEngineCore.Entities;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Batches;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Shaders;
using SpiceEngineCore.Rendering.Vertices;
using StarchUICore.Views;
using SweetGraphicsCore.Buffers;
using SweetGraphicsCore.Vertices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StarchUICore.Rendering.Batches
{
    public class LabelBatch : Batch<Label>
    {
        private VertexBuffer<TextureVertex2D> _vertexBuffer = new VertexBuffer<TextureVertex2D>();
        private VertexArray<TextureVertex2D> _vertexArray = new VertexArray<TextureVertex2D>();

        private Dictionary<int, int> _offsetByID = new Dictionary<int, int>();
        private Dictionary<int, int> _countByID = new Dictionary<int, int>();

        public LabelBatch(Label item) : base(item) { }

        public override void Load()
        {
            _vertexBuffer = new VertexBuffer<TextureVertex2D>();
            _vertexArray = new VertexArray<TextureVertex2D>();

            _vertexBuffer.Clear();
            //_vertexBuffer.AddVertices(_vertexSet.Vertices);

            _vertexBuffer.Bind();
            _vertexArray.Load();
            _vertexBuffer.Unbind();

            //base.Load();
            IsLoaded = true;
        }

        public override IBatch Duplicate() => throw new NotImplementedException("Cannot handle text renderable of type " + _renderable.GetType());

        public override void AddEntity(int id, IRenderable renderable)
        {
            if (renderable is Label label)
            {
                label.TextChanged += (s, args) =>
                {
                    if (!_offsetByID.ContainsKey(id))
                    {
                        _offsetByID.Add(id, _vertexBuffer.Count);
                        _countByID.Add(id, args.Vertices.Count);
                        _vertexBuffer.AddVertices(args.Vertices);
                    }
                    else
                    {
                        var offset = _offsetByID[id];
                        var count = _countByID[id];

                        // TODO - For now, make it a requirement that the counts match
                        if (count != args.Vertices.Count) throw new ArgumentOutOfRangeException("Cannot handle vertex count change for TextBatch");
                        
                        for (var i = 0; i < args.Vertices.Count; i++)
                        {
                            _vertexBuffer.SetVertex(offset + i, args.Vertices[i]);
                        }
                    }
                };

                // TODO - This order within the vertex buffer needs to be altered to match the draw order from the UI Provider
                //textView.LayoutChanged += (s, args) =>
            }

            base.AddEntity(id, renderable);
        }

        public void Reorder(IList<int> orderedIDs)
        {
            /*if (IsLoaded)
            {
                var vertices = new List<TextureVertex2D>();

                foreach (var id in orderedIDs)
                {
                    if (EntityIDs.Contains(id) && _offsetByID.ContainsKey(id))
                    {
                        var offset = _offsetByID[id];
                        _offsetByID[id] = vertices.Count;

                        var vertex = _vertexBuffer.GetVertex(offset);
                        vertices.Add(vertex);
                    }
                }

                _vertexBuffer.Clear();
                _vertexBuffer.AddVertices(vertices);
            }*/
        }

        public override void Draw()
        {
            _vertexArray.Bind();
            _vertexBuffer.Bind();

            _vertexBuffer.Buffer();
            _vertexBuffer.DrawQuads();

            _vertexArray.Unbind();
            _vertexBuffer.Unbind();

            //if (IsVisible && Measurement.Width > 0 && Measurement.Height > 0)
        }

        public override void Transform(int entityID, Transform transform)
        {
            // TODO - This is redundant with function overloads for Mesh.Transform()
            var offset = _offsetByID.ContainsKey(entityID) ? _offsetByID[entityID] : 0;
            //var count = _countByID.ContainsKey(entityID) ? _countByID[entityID] : _renderable.Vertices.Count();

            //_renderable.Transform(transform, offset, count);
        }

        public override void UpdateVertices(int entityID, Func<IVertex, IVertex> vertexUpdate)
        {
            // TODO - This is redundant with function overloads for Mesh.Transform()
            var offset = _offsetByID.ContainsKey(entityID) ? _offsetByID[entityID] : 0;
            //var count = _countByID.ContainsKey(entityID) ? _countByID[entityID] : _renderable.Vertices.Count();

            //_renderable.Update(vertexUpdate, offset, count);
        }

        public override bool CompareUniforms(IRenderable renderable) => renderable is Label textView && textView.Font == _renderable.Font;

        public override void SetUniforms(IEntityProvider entityProvider, ShaderProgram shaderProgram)
        {
            // TODO - Are there any per entity uniforms that we actually need to set?
            var entity = entityProvider.GetEntity(EntityIDs.First());
            entity.WorldMatrix.Set(shaderProgram);

            //shaderProgram.BindTexture(_renderable.Font.Texture, "textureSampler", 0);
            shaderProgram.SetUniform("color", _renderable.Color);
        }
    }
}
