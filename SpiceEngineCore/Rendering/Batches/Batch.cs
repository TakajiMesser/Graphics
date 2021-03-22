using SpiceEngineCore.Entities;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Shaders;
using SpiceEngineCore.Rendering.Textures;
using SpiceEngineCore.Rendering.Vertices;
using System;
using System.Collections.Generic;

namespace SpiceEngineCore.Rendering.Batches
{
    public abstract class Batch<T> : IBatch where T : IRenderable
    {
        protected T _renderable;
        private List<int> _entityIDs = new List<int>();

        public Batch(T renderable) => _renderable = renderable;

        public IEnumerable<int> EntityIDs => _entityIDs;
        public int EntityCount => _entityIDs.Count;
        public bool IsLoaded { get; protected set; }

        public virtual void AddEntity(int id, IRenderable renderable)
        {
            _entityIDs.Add(id);
            IsLoaded = false;
        }

        public virtual void Transform(int entityID, Transform transform) { }
        public virtual void TransformTexture(int entityID, Vector3 center, Vector2 translation, float rotation, Vector2 scale) { }

        public virtual void UpdateVertices(int entityID, Func<IVertex, IVertex> vertexUpdate) { }

        public virtual void RemoveEntity(int id) => _entityIDs.Remove(id);

        public abstract bool CanBatch(IRenderable renderable);

        public virtual void Load(IRenderContextProvider contextProvider)
        {
            _renderable.Load(contextProvider);
            IsLoaded = true;
        }

        public virtual void Draw(IShader shader, IEntityProvider entityProvider, ITextureProvider textureProvider = null) => _renderable.Draw();

        public abstract IBatch Duplicate();
    }
}
