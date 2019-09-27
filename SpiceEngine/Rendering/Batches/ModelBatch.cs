using SpiceEngine.Entities;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;
using System;
using System.Linq;

namespace SpiceEngine.Rendering.Batches
{
    public class ModelBatch : Batch
    {
        public Model Model { get; }

        public ModelBatch(Model model) => Model = model;

        public override IBatch Duplicate() => new ModelBatch(Model.Duplicate());

        public override void UpdateVertices(int entityID, Func<IVertex3D, IVertex3D> vertexUpdate)
        {
            foreach (var mesh in Model.Meshes)
            {
                mesh.Update(vertexUpdate);
            }
        }

        public override void Load() => Model.Load();

        public override void Draw(IEntityProvider entityProvider, ShaderProgram shaderProgram, ITextureProvider textureProvider = null)
        {
            var entity = entityProvider.GetEntity(EntityIDs.First());
            var textureBinder = textureProvider != null ? entity as ITextureBinder : null;

            for (var i = 0; i < Model.Meshes.Count; i++)
            {
                ((IModel)entity).SetMeshIndex(i);

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
