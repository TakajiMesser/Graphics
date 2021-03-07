using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Brushes;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Batches;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Shaders;
using SpiceEngineCore.Rendering.Textures;
using SpiceEngineCore.Rendering.Vertices;
using SweetGraphicsCore.Rendering.Meshes;
using SweetGraphicsCore.Rendering.Models;
using System;
using System.Linq;

namespace SweetGraphicsCore.Rendering.Batches
{
    public class ModelBatch : Batch<IModel>
    {
        public ModelBatch(IModel model) : base(model) { }

        public override IBatch Duplicate() => new ModelBatch(_renderable.Duplicate());

        public override void UpdateVertices(int entityID, Func<IVertex, IVertex> vertexUpdate)
        {
            foreach (var mesh in _renderable.Meshes)
            {
                mesh.Update(vertexUpdate);
            }
        }

        // For now, assume that models are never good candidates for combining batches,
        // as their positions must get updated too frequently
        public override bool CanBatch(IRenderable renderable) => false;

        public override void Draw(IShader shader, IEntityProvider entityProvider, ITextureProvider textureProvider = null)
        {
            var entity = entityProvider.GetEntity(EntityIDs.First());

            // TODO - This is janky to set this uniform based on entity type...
            if (entity is IBrush)
            {
                shader.SetUniform(ModelMatrix.CURRENT_NAME, Matrix4.Identity);
                shader.SetUniform(ModelMatrix.PREVIOUS_NAME, Matrix4.Identity);
            }
            else
            {
                shader.SetUniform(ModelMatrix.CURRENT_NAME, entity.CurrentModelMatrix);
                shader.SetUniform(ModelMatrix.PREVIOUS_NAME, entity.PreviousModelMatrix);
            }

            for (var i = 0; i < _renderable.Meshes.Count; i++)
            {
                if (_renderable.Meshes[i] is ITexturedMesh texturedMesh)
                {
                    shader.SetMaterial(texturedMesh.Material);

                    if (textureProvider != null)
                    {
                        if (texturedMesh.TextureMapping.HasValue)
                        {
                            shader.BindTextures(textureProvider, texturedMesh.TextureMapping.Value);
                        }
                        else
                        {
                            shader.UnbindTextures();
                        }
                    }
                }

                _renderable.SetUniforms(shader, i);
                _renderable.Meshes[i].Draw();
            }
        }
    }
}
