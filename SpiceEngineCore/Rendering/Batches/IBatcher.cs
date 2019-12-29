using SpiceEngineCore.Components.Animations;
using SpiceEngineCore.Rendering.Vertices;
using System;

namespace SpiceEngineCore.Rendering.Batches
{
    public interface IBatcher
    {
        bool IsLoaded { get; }

        IBatch GetBatch(int entityID);
        IBatch GetBatchOrDefault(int entityID);

        void SetAnimationProvider(IAnimationProvider animationProvider);

        void AddEntity(int entityID, IRenderable renderable);
        void RemoveByEntityID(int entityID);

        void Load();
        void Load(int entityID);

        void UpdateVertices(int entityID, Func<IVertex, IVertex> vertexUpdate);
        void DuplicateBatch(int entityID, int newID);

        IBatchAction CreateBatchAction();
    }
}
