using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Brushes;
using SpiceEngineCore.Game;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Batches;
using SpiceEngineCore.Rendering.Materials;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Textures;
using SpiceEngineCore.Rendering.Vertices;
using SweetGraphicsCore.Rendering.Meshes;
using SweetGraphicsCore.Rendering.Models;
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

namespace SweetGraphicsCore.Rendering.Batches
{
    // TODO - This is a shit class, since it isn't actually a proper batch (might consist of multiple set-uniform steps and draw calls)
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

        // For now, assume that models are never good candidates for combining batches, as their positions must get updated too frequently
        public override bool CanBatch(IRenderable renderable) => false;

        /*public override IEnumerable<IUniform> GetUniforms(IBatcher batcher)
        {
            var entity = batcher.GetEntitiesForBatch(this).First();

            yield return new Uniform<Matrix4>(ModelMatrix.CURRENT_NAME, entity.WorldMatrix.CurrentValue);
            yield return new Uniform<Matrix4>(ModelMatrix.PREVIOUS_NAME, entity.WorldMatrix.PreviousValue);

            if (_renderable.Meshes[_drawIndex] is ITexturedMesh texturedMesh)
            {
                yield return new Uniform<Vector3>(Material.AMBIENT_NAME, texturedMesh.Material.Ambient);
                yield return new Uniform<Vector3>(Material.DIFFUSE_NAME, texturedMesh.Material.Diffuse);
                yield return new Uniform<Vector3>(Material.SPECULAR_NAME, texturedMesh.Material.Specular);
                yield return new Uniform<float>(Material.SPECULAR_EXPONENT_NAME, texturedMesh.Material.SpecularExponent);
            }
        }

        public override IEnumerable<TextureBinding> GetTextureBindings(ITextureProvider textureProvider)
        {
            if (_renderable.Meshes[_drawIndex] is ITexturedMesh texturedMesh && texturedMesh.TextureMapping.HasValue)
            {
                var diffuseTexture = textureProvider.RetrieveTexture(texturedMesh.TextureMapping.Value.DiffuseIndex);
                yield return new TextureBinding(TextureMapping.DIFFUSE_NAME, diffuseTexture);

                var normalTexture = textureProvider.RetrieveTexture(texturedMesh.TextureMapping.Value.NormalIndex);
                yield return new TextureBinding(TextureMapping.NORMAL_NAME, normalTexture);

                var specularTexture = textureProvider.RetrieveTexture(texturedMesh.TextureMapping.Value.SpecularIndex);
                yield return new TextureBinding(TextureMapping.SPECULAR_NAME, specularTexture);

                var parallaxTexture = textureProvider.RetrieveTexture(texturedMesh.TextureMapping.Value.ParallaxIndex);
                yield return new TextureBinding(TextureMapping.PARALLAX_NAME, parallaxTexture);
            }
        }*/

        public override void SetUniforms(IRender renderer, IEntityProvider entityProvider)
        {
            if (_renderable.Meshes[_drawIndex] is ITexturedMesh texturedMesh)
            {
                renderer.SetMaterial(texturedMesh.Material);
            }

            //_renderable.SetUniforms(renderer, _drawIndex);
        }

        public override void BindTextures(IRender renderer, ITextureProvider textureProvider)
        {
            if (_renderable.Meshes[_drawIndex] is ITexturedMesh texturedMesh)
            {
                if (texturedMesh.TextureMapping.HasValue)
                {
                    renderer.BindTextures(textureProvider, texturedMesh.TextureMapping.Value);
                }
                else
                {
                    renderer.UnbindTextures();
                }
            }
        }

        public override void Draw()
        {
            _renderable.Meshes[_drawIndex].Draw();
            //_drawIndex++;
            //_drawIndex %= _renderable.Meshes.Count;
        }

        public override void Draw(IRender renderer, IEntityProvider entityProvider, ITextureProvider textureProvider = null)
        {
            var entity = entityProvider.GetEntity(EntityIDs.First());

            // TODO - This is janky to set this uniform based on entity type...
            if (entity is IBrush)
            {
                renderer.SetUniform(ModelMatrix.CURRENT_NAME, Matrix4.Identity);
                renderer.SetUniform(ModelMatrix.PREVIOUS_NAME, Matrix4.Identity);
            }
            else
            {
                renderer.SetUniform(ModelMatrix.CURRENT_NAME, entity.WorldMatrix.CurrentValue);
                renderer.SetUniform(ModelMatrix.PREVIOUS_NAME, entity.WorldMatrix.PreviousValue);
                //entity.WorldMatrix.Set(shaderProgram);
            }

            for (var i = 0; i < _renderable.Meshes.Count; i++)
            {
                _drawIndex = i;
                base.Draw(renderer, entityProvider, textureProvider);
            }
        }
    }
}
