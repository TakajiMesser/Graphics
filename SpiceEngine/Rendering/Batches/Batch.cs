using SpiceEngine.Entities;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;
using System.Collections.Generic;

namespace SpiceEngine.Rendering.Batches
{
    public abstract class Batch : IBatch
    {
        private List<int> _entityIDs = new List<int>();

        public IEnumerable<int> EntityIDs => _entityIDs;

        public void AddEntity(int id)
        {
            _entityIDs.Add(id);
        }

        public void RemoveEntity(int id)
        {
            _entityIDs.Remove(id);
        }

        public abstract void Load();
        public abstract void Draw(IEntityProvider entityProvider, ShaderProgram shaderProgram, ITextureProvider textureProvider = null);
        public abstract IBatch Duplicate();
    }
}
