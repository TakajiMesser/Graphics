using OpenTK;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Brushes;
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
        private int _drawIndex = 0;

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
        public override bool CompareUniforms(IRenderable renderable) => false;

        public override void SetUniforms(IEntityProvider entityProvider, ShaderProgram shaderProgram)
        {
            if (_renderable.Meshes[_drawIndex] is ITexturedMesh texturedMesh)
            {
                texturedMesh.Material.SetUniforms(shaderProgram);
            }

            if (_renderable is IAnimatedModel animatedModel)
            {
                shaderProgram.SetUniform("jointTransforms", animatedModel.GetJointTransforms(_drawIndex));
            }
        }

        public override void BindTextures(ShaderProgram shaderProgram, ITextureProvider textureProvider)
        {
            if (_renderable.Meshes[_drawIndex] is ITexturedMesh texturedMesh)
            {
                if (texturedMesh.TextureMapping.HasValue)
                {
                    shaderProgram.BindTextures(textureProvider, texturedMesh.TextureMapping.Value);
                }
                else
                {
                    shaderProgram.UnbindTextures();
                }
            }
        }

        public override void Draw() => _renderable.Meshes[_drawIndex].Draw();

        public override void Draw(IEntityProvider entityProvider, ShaderProgram shaderProgram, ITextureProvider textureProvider = null)
        {
            var entity = entityProvider.GetEntity(EntityIDs.First());

            // TODO - This is janky to set this uniform based on entity type...
            if (entity is IBrush)
            {
                shaderProgram.SetUniform(ModelMatrix.NAME, Matrix4.Identity);
                shaderProgram.SetUniform(ModelMatrix.PREVIOUS_NAME, Matrix4.Identity);
            }
            else
            {
                entity.WorldMatrix.Set(shaderProgram);
            }

            for (var i = 0; i < _renderable.Meshes.Count; i++)
            {
                _drawIndex = i;
                base.Draw(entityProvider, shaderProgram, textureProvider);
            }
        }
    }
}
