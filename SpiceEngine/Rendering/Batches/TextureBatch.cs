using SpiceEngine.Entities;
using SpiceEngine.Entities.Brushes;
using SpiceEngine.Entities.Volumes;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;
using System.Collections.Generic;

namespace SpiceEngine.Rendering.Batches
{
    public class TextureBatch : IBatch
    {
        private List<Vector3> _positions = new List<Vector3>();

        public TextureBatch(int entityID, Texture texture)
        {
            EntityID = entityID;
            Texture = texture;
        }

        public int EntityID { get; private set; }
        public Texture Texture { get; private set; }

        public IBatch Duplicate(int entityID) => new MeshBatch(entityID, Mesh.Duplicate());

        public void Load()
        {

        }

        public void Draw(IEntityProvider entityProvider, ShaderProgram shaderProgram, TextureManager textureManager = null)
        {
            var entity = entityProvider.GetEntity(EntityID);
            entity.Position;

            shaderProgram.SetTexture(Texture, "mainTexture", 0);

            switch (entity)
            {
                case Brush brush:
                    brush.SetUniforms(shaderProgram, textureManager);
                    break;
                case Volume volume:
                    volume.SetUniforms(shaderProgram);
                    break;
            }
            
            Mesh.Draw();
        }
    }
}
