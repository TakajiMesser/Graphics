using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Helpers;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Batches;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Shaders;
using SpiceEngineCore.Rendering.Vertices;
using StarchUICore.Groups;
using StarchUICore.Rendering.Vertices;
using StarchUICore.Views;
using SweetGraphicsCore.Buffers;
using SweetGraphicsCore.Vertices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StarchUICore.Rendering.Batches
{
    public class UIBatch : Batch<IElement>
    {
        //private Vertex3DSet<ViewQuadVertex> _vertexSet = new Vertex3DSet<ViewQuadVertex>();

        private VertexBuffer<ViewQuadVertex> _vertexBuffer;
        private VertexArray<ViewQuadVertex> _vertexArray;

        private Dictionary<int, int> _offsetByID = new Dictionary<int, int>();
        private Dictionary<int, int> _countByID = new Dictionary<int, int>();

        public UIBatch(IElement item) : base(item) { }

        public override void Load()
        {
            _vertexBuffer = new VertexBuffer<ViewQuadVertex>();
            _vertexArray = new VertexArray<ViewQuadVertex>();

            _vertexBuffer.Clear();
            //_vertexBuffer.AddVertices(_vertexSet.Vertices);

            _vertexBuffer.Bind();
            _vertexArray.Load();
            _vertexBuffer.Unbind();

            //base.Load();
            IsLoaded = true;
        }

        public override IBatch Duplicate()
        {
            if (_renderable is IGroup group)
            {
                return new UIBatch(group.Duplicate());
            }
            else if (_renderable is IView view)
            {
                return new UIBatch(view.Duplicate());
            }

            throw new NotImplementedException("Cannot handle UI renderable of type " + _renderable.GetType());
        }

        public override void AddEntity(int id, IRenderable renderable)
        {
            if (renderable is IElement element)
            {
                if (renderable is IView view)
                {
                    AddView(id, view);
                }
                else if (renderable is IGroup group)
                {
                    // TODO - Handle adding groups
                    /*foreach (var item in group.Children)
                    {

                    }*/
                }

                var offset = _offsetByID.ContainsKey(id) ? _offsetByID[id] : 0;
                var count = _countByID.ContainsKey(id) ? _countByID[id] : 1;

                element.LayoutChanged += (s, args) =>
                {
                    var name = element.Name;

                    // TODO - Handle omitting views that are not visible or have measurement dimensions of zero
                    var position = new Vector3(element.Location.X, element.Location.Y, 0.0f);
                    var borderThickness = element.Border.Thickness;
                    var size = new Vector2(element.Measurement.Width, element.Measurement.Height);
                    var cornerRadius = new Vector2(element.Border.CornerXRadius, element.Border.CornerYRadius);
                    var color = renderable is IView view2
                        ? new Color4(view2.Background.Color.R, view2.Background.Color.G, view2.Background.Color.B, element.Alpha)
                        : new Color4(1.0f, 1.0f, 1.0f, element.Alpha);
                    var borderColor = element.Border.Color;
                    var selectionID = SelectionHelper.GetColorFromID(id);

                    if (element is IView)
                    {
                        var vertex = new ViewQuadVertex(position, borderThickness, size, cornerRadius, color, borderColor, selectionID);
                        //_vertexBuffer.Clear();

                        if (offset >= _vertexBuffer.Count)
                        {
                            _vertexBuffer.AddVertex(vertex);
                        }
                        else
                        {
                            _vertexBuffer.SetVertex(offset, vertex);
                        }
                    }
                };
            }

            base.AddEntity(id, renderable);
        }

        private void AddView(int id, IView view)
        {
            /*if (EntityIDs.Any())
            {*/
                // TODO - For testing purposes, let's assume that each view only has a single vertex
                var offset = 1;// view.Vertices.Count();
                _offsetByID.Add(id, offset);
                _countByID.Add(id, offset + 1);// view.Vertices.Count());

                //_renderable.Combine(view);
            /*}
            else
            {
                _offsetByID.Add(id, 0);
                _countByID.Add(id, 1);// view.Vertices.Count());
            }*/
        }

        public override void Draw()
        {
            _vertexArray.Bind();
            _vertexBuffer.Bind();

            _vertexBuffer.Buffer();
            GL.DrawArrays(PrimitiveType.Points, 0, _vertexBuffer.Count);

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

        public override bool CompareUniforms(IRenderable renderable)
        {
            // TODO - Determine if we do actually want to support batching for UI items
            if (renderable is IView)
            {
                return true;
            }

            return false;
        }

        public override void SetUniforms(IEntityProvider entityProvider, ShaderProgram shaderProgram)
        {
            // TODO - Are there any per entity uniforms that we actually need to set?
            var entity = entityProvider.GetEntity(EntityIDs.First());
            entity.WorldMatrix.Set(shaderProgram);

            // TODO - This is janky to set this uniform based on entity type...
            /*if (entity is IBrush)
            {
                shaderProgram.SetUniform(ModelMatrix.NAME, Matrix4.Identity);
                shaderProgram.SetUniform(ModelMatrix.PREVIOUS_NAME, Matrix4.Identity);
            }
            else
            {
                entity.WorldMatrix.Set(shaderProgram);
            }*/
        }
    }
}
