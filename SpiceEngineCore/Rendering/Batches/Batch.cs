using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Textures;
using SpiceEngineCore.Rendering.Vertices;
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

namespace SpiceEngineCore.Rendering.Batches
{
    public abstract class Batch<T> : IBatch where T : IRenderable
    {
        protected T _renderable;
        private List<int> _entityIDs = new List<int>();

        public Batch(T renderable) => _renderable = renderable;

        public IRenderable Renderable => _renderable;
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

        public virtual void Load()
        {
            _renderable.Load();
            IsLoaded = true;
        }

        public abstract bool CanBatch(IRenderable renderable);

        public virtual IEnumerable<IUniform> GetUniforms(IBatcher batcher) => Enumerable.Empty<IUniform>();
        public virtual IEnumerable<TextureBinding> GetTextureBindings(ITextureProvider textureProvider) => Enumerable.Empty<TextureBinding>();
        
        public virtual void Draw() => _renderable.Draw();

        /*public abstract void SetUniforms(IRender renderer, IEntityProvider entityProvider);
        public virtual void BindTextures(IRender renderer, ITextureProvider textureProvider) { }
        public virtual void Draw(IRender renderer, IEntityProvider entityProvider, ITextureProvider textureProvider = null)
        {
            SetUniforms(renderer, entityProvider);

            if (textureProvider != null)
            {
                BindTextures(renderer, textureProvider);
            }

            Draw();
        }*/

        public abstract IBatch Duplicate();
    }
}
