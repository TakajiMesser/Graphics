using SpiceEngine.Entities;
using SpiceEngine.Entities.Actors;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Rendering.Batches
{
    public class ModelBatch : Batch
    {
        public Model Model { get; }

        public ModelBatch(Model model) => Model = model;

        public override IBatch Duplicate() => new ModelBatch(Model.Duplicate());

        public override void Load() => Model.Load();

        public override void Draw(IEntityProvider entityProvider, ShaderProgram shaderProgram, ITextureProvider textureProvider = null)
        {
            var actor = (Actor)entityProvider.GetEntity(EntityIDs.First());
            actor.SetUniforms(shaderProgram);

            int meshIndex = 0;

            foreach (var mesh in Model.Meshes)
            {
                actor.SetUniforms(shaderProgram, textureProvider, meshIndex);
                mesh.Draw();
                meshIndex++;
            }
        }
    }
}
