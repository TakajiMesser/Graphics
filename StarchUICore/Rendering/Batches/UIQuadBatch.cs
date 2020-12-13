using OpenTK.Graphics.OpenGL;
using SpiceEngineCore.Helpers;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Batches;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Vertices;
using StarchUICore.Groups;
using StarchUICore.Rendering.Vertices;
using StarchUICore.Views;
using SweetGraphicsCore.Buffers;
using SweetGraphicsCore.Vertices;
using System;
using System.Collections.Generic;
using System.Linq;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace StarchUICore.Rendering.Batches
{
    public class UIQuadBatch : Batch<IElement>
    {
        //private Vertex3DSet<ViewQuadVertex> _vertexSet = new Vertex3DSet<ViewQuadVertex>();

        private VertexBuffer<ViewQuadVertex> _vertexBuffer;
        private VertexArray<ViewQuadVertex> _vertexArray;

        private Dictionary<int, int> _offsetByID = new Dictionary<int, int>();
        private Dictionary<int, int> _countByID = new Dictionary<int, int>();

        public UIQuadBatch(IElement item) : base(item) { }

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
                return new UIQuadBatch(group.Duplicate());
            }
            else if (_renderable is IView view)
            {
                return new UIQuadBatch(view.Duplicate());
            }

            throw new NotImplementedException("Cannot handle UI renderable of type " + _renderable.GetType());
        }

        public override void AddEntity(int id, IRenderable renderable)
        {
            if (renderable is IElement element)
            {
                // TODO - For now, let's also add groups for debugging purposes. This should eventually be a toggle
                /*if (renderable is IView || renderable is IGroup group)
                {
                    
                }*/

                // TODO - This order within the vertex buffer needs to be altered to match the draw order from the UI Provider
                element.MeasurementChanged += (s, args) =>
                {
                    var idColor = SelectionHelper.GetColorFromID(id);
                    var vertex = new ViewQuadVertex(args.Position, args.BorderThickness, args.Size, args.CornerRadius, args.Color, args.BorderColor, idColor);

                    //var count = _countByID[id];

                    if (!_offsetByID.ContainsKey(id))
                    {
                        _offsetByID.Add(id, _vertexBuffer.Count);
                        _countByID.Add(id, 1);
                        _vertexBuffer.AddVertex(vertex);
                    }
                    else
                    {
                        var offset = _offsetByID[id];
                        _vertexBuffer.SetVertex(offset, vertex);
                    }

                    /*if (offset >= _vertexBuffer.Count)
                    {
                        _vertexBuffer.AddVertex(vertex);
                    }
                    else
                    {
                        _vertexBuffer.SetVertex(offset, vertex);
                    }*/
                };
            }

            base.AddEntity(id, renderable);
        }

        public void Reorder(IList<int> orderedIDs)
        {
            if (IsLoaded)
            {
                var vertices = new List<ViewQuadVertex>();

                foreach (var id in orderedIDs)
                {
                    if (EntityIDs.Contains(id))
                    {
                        var offset = _offsetByID[id];
                        _offsetByID[id] = vertices.Count;

                        var vertex = _vertexBuffer.GetVertex(offset);
                        vertices.Add(vertex);
                    }
                }

                _vertexBuffer.Clear();
                _vertexBuffer.AddVertices(vertices);
            }
        }

        /*private void HandleLayoutChange(int id, IElement element, IRenderable renderable)
        {
            // TODO - Handle omitting views that are not visible or have measurement dimensions of zero
            var position = new Vector3(element.Location.X, element.Location.Y, 0.0f);
            var borderThickness = element.Border.Thickness;
            var size = new Vector2(element.Measurement.Width, element.Measurement.Height);
            var cornerRadius = new Vector2(element.Border.CornerXRadius, element.Border.CornerYRadius);
            var color = renderable is IView view
                ? new Color4(view.Background.Color.R, view.Background.Color.G, view.Background.Color.B, element.Alpha)
                : new Color4(1.0f, 1.0f, 1.0f, element.Alpha);
            var borderColor = element.Border.Color;
            var selectionID = SelectionHelper.GetColorFromID(id);


            // TODO - Is there any way we can determine the "reordering strategy" in this event handler?
            // If this view's layout just changed, but it isn't currently 

            // Determine the offset/count of this element's vertices in the instance buffer
            var offset = _offsetByID.ContainsKey(id) ? _offsetByID[id] : 0;
            var count = _countByID.ContainsKey(id) ? _countByID[id] : 1;

            // For now, let's assume all views and groups have just a single vertex for rendering
            // SO, the offset here determines what index in the vertex buffer this element ID starts at
            if (EntityIDs.Any())
            {
                var offset = _vertexBuffer.Count;
                _offsetByID.Add(id, offset);
                _countByID.Add(id, offset + 1);
            }
            else
            {
                _offsetByID.Add(id, 0);
                _countByID.Add(id, mesh.Vertices.Count());
            }

            if (element is IView)
            {
                var vertex = new ViewQuadVertex(position, borderThickness, size, cornerRadius, color, borderColor, selectionID);

                if (offset >= _vertexBuffer.Count)
                {
                    _vertexBuffer.AddVertex(vertex);
                }
                else
                {
                    _vertexBuffer.SetVertex(offset, vertex);
                }
            }
            else if (element is IGroup)
            {
                // For now, let's also draw group borders for debugging purposes...
                var vertex = new ViewQuadVertex(position, 2.0f, size, Vector2.Zero, Color4.Transparent, Color4.White, selectionID);

                if (offset >= _vertexBuffer.Count)
                {
                    // Let's insert groups at the start of the buffer to ensure that views get drawn after them
                    _vertexBuffer.InsertVertex(0, vertex);
                }
                else
                {
                    _vertexBuffer.SetVertex(offset, vertex);
                }
            }
        }*/

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

        // TODO - Determine if we do actually want to support batching for UI items
        public override bool CanBatch(IRenderable renderable) => renderable is IView || renderable is IGroup;

        public override IEnumerable<IUniform> GetUniforms(IBatcher batcher)
        {
            var entity = batcher.GetEntitiesForBatch(this).First();

            yield return new Uniform<Matrix4>(ModelMatrix.CURRENT_NAME, entity.WorldMatrix.CurrentValue);
            yield return new Uniform<Matrix4>(ModelMatrix.PREVIOUS_NAME, entity.WorldMatrix.PreviousValue);
        }
    }
}
