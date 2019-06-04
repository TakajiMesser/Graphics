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
    public class ModelBatch : IBatch
    {
        public int EntityID { get; private set; }
        public Model Model { get; }
        public IEnumerable<IVertex3D> Vertices => Model?.Meshes.SelectMany(m => m.Vertices);

        public ModelBatch(int entityID, Model model)
        {
            EntityID = entityID;
            Model = model;
        }

        public IBatch Duplicate(int entityID) => new ModelBatch(entityID, Model.Duplicate());

        public void Load() => Model.Load();

        public void Draw(IEntityProvider entityProvider, ShaderProgram shaderProgram, TextureManager textureManager = null)
        {
            var actor = (Actor)entityProvider.GetEntity(EntityID);
            actor.SetUniforms(shaderProgram, textureManager);

            int meshIndex = 0;

            foreach (var mesh in Model.Meshes)
            {
                actor.SetUniforms(shaderProgram, textureManager, meshIndex);
                mesh.Draw();
                meshIndex++;
            }
        }
    }
}
