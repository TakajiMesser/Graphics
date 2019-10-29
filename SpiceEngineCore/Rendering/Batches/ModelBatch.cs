using SpiceEngineCore.Entities;
using SpiceEngineCore.Rendering.Models;
using SpiceEngineCore.Rendering.Shaders;
using SpiceEngineCore.Rendering.Textures;
using SpiceEngineCore.Rendering.Vertices;
using System;
using System.Linq;

namespace SpiceEngineCore.Rendering.Batches
{
    public class ModelBatch : Batch
    {
        public IModel Model { get; }

        public ModelBatch(IModel model) => Model = model;

        public override IBatch Duplicate() => new ModelBatch(Model.Duplicate());

        public override void UpdateVertices(int entityID, Func<IVertex3D, IVertex3D> vertexUpdate)
        {
            foreach (var mesh in Model.Meshes)
            {
                mesh.Update(vertexUpdate);
            }
        }

        public override void Load()
        {
            Model.Load();
            base.Load();
        }

        public override void Draw(IEntityProvider entityProvider, ShaderProgram shaderProgram, ITextureProvider textureProvider = null)
        {
            var entity = entityProvider.GetEntity(EntityIDs.First());
            var textureBinder = textureProvider != null ? entity as ITextureBinder : null;

            for (var i = 0; i < Model.Meshes.Count; i++)
            {
                ((IHaveModel)entity).SetMeshIndex(i);

                if (textureBinder != null)
                {
                    // TODO - Determine when to unbind textures
                    textureBinder.BindTextures(shaderProgram, textureProvider);
                }

                entity.SetUniforms(shaderProgram);
                Model.Meshes[i].Draw();
            }
        }
    }
}
