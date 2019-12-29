using OpenTK;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Brushes;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Shaders;
using SpiceEngineCore.Rendering.UserInterfaces.Views;
using SpiceEngineCore.Rendering.Vertices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngineCore.Rendering.Batches
{
    public class ViewBatch : Batch<IUIView>
    {
        private Dictionary<int, int> _offsetByID = new Dictionary<int, int>();
        private Dictionary<int, int> _countByID = new Dictionary<int, int>();

        public ViewBatch(IUIView view) : base(view) { }

        public override IBatch Duplicate() => new ViewBatch(_renderable.Duplicate());

        public override void AddEntity(int id, IRenderable renderable)
        {
            if (renderable is IUIView view)
            {
                if (EntityIDs.Any())
                {
                    var offset = _renderable.Vertices.Count();
                    _offsetByID.Add(id, offset);
                    _countByID.Add(id, offset + view.Vertices.Count());

                    //_renderable.Combine(view);
                }
                else
                {
                    _offsetByID.Add(id, 0);
                    _countByID.Add(id, view.Vertices.Count());
                }
            }

            base.AddEntity(id, renderable);
        }

        public override void Transform(int entityID, Transform transform)
        {
            // TODO - This is redundant with function overloads for Mesh.Transform()
            var offset = _offsetByID.ContainsKey(entityID) ? _offsetByID[entityID] : 0;
            var count = _countByID.ContainsKey(entityID) ? _countByID[entityID] : _renderable.Vertices.Count();

            //_renderable.Transform(transform, offset, count);
        }

        public override void UpdateVertices(int entityID, Func<IVertex, IVertex> vertexUpdate)
        {
            // TODO - This is redundant with function overloads for Mesh.Transform()
            var offset = _offsetByID.ContainsKey(entityID) ? _offsetByID[entityID] : 0;
            var count = _countByID.ContainsKey(entityID) ? _countByID[entityID] : _renderable.Vertices.Count();

            //_renderable.Update(vertexUpdate, offset, count);
        }

        public override bool CompareUniforms(IRenderable renderable)
        {
            if (renderable is IUIView)
            {
                return true;
            }

            return false;
        }

        public override void SetUniforms(IEntityProvider entityProvider, ShaderProgram shaderProgram)
        {
            // TODO - Are there any per entity uniforms that we actually need to set?
            /*var entity = entityProvider.GetEntity(EntityIDs.First());

            // TODO - This is janky to set this uniform based on entity type...
            if (entity is IBrush)
            {*/
                shaderProgram.SetUniform(ModelMatrix.NAME, Matrix4.Identity);
                shaderProgram.SetUniform(ModelMatrix.PREVIOUS_NAME, Matrix4.Identity);
            /*}
            else
            {
                entity.WorldMatrix.Set(shaderProgram);
            }*/
        }
    }
}
