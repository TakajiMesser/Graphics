using OpenTK;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Batches;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Shaders;
using SpiceEngineCore.Rendering.Vertices;
using StarchUICore.Groups;
using StarchUICore.Views;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StarchUICore.Rendering.Batches
{
    public class UIBatch : Batch<IUIItem>
    {
        private Dictionary<int, int> _offsetByID = new Dictionary<int, int>();
        private Dictionary<int, int> _countByID = new Dictionary<int, int>();

        public UIBatch(IUIItem item) : base(item) { }

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
            if (renderable is IView view)
            {
                AddView(id, view);
            }
            else if (renderable is IGroup group)
            {
                // TODO - Handle adding groups
                foreach (var item in group.GetChildren())
                {

                }
            }

            base.AddEntity(id, renderable);
        }

        private void AddView(int id, IView view)
        {
            if (EntityIDs.Any())
            {
                var offset = view.Vertices.Count();
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
            /*if (renderable is IUIView)
            {
                return true;
            }*/

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
