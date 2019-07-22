using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;
using System;
using System.Collections.Generic;

namespace SpiceEngine.Rendering.Batches
{
    public abstract class Batch : IBatch
    {
        private List<int> _entityIDs = new List<int>();

        public IEnumerable<int> EntityIDs => _entityIDs;

        public virtual void AddEntity(int id, IRenderable renderable)
        {
            _entityIDs.Add(id);
        }

        public virtual void Transform(int entityID, Matrix4 matrix) { }

        public virtual void TransformTexture(int entityID, Vector3 center, Vector2 translation, float rotation, Vector2 scale) { }

        public virtual void UpdateVertices(int entityID, Func<IVertex3D, IVertex3D> vertexUpdate) { }

        public virtual void RemoveEntity(int id)
        {
            _entityIDs.Remove(id);
        }

        public abstract void Load();
        public abstract void Draw(IEntityProvider entityProvider, ShaderProgram shaderProgram, ITextureProvider textureProvider = null);
        public abstract IBatch Duplicate();
    }
}
