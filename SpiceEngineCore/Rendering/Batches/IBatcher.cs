using SpiceEngineCore.Entities;
using SpiceEngineCore.Rendering.Vertices;
using System;
using System.Collections.Generic;

namespace SpiceEngineCore.Rendering.Batches
{
    public interface IBatcher
    {
        bool IsLoaded { get; }

        void AddEntity(int entityID, IRenderable renderable);
        void RemoveByEntityID(int entityID);

        void Load();
        void Load(int entityID);

        void UpdateVertices(int entityID, Func<IVertex, IVertex> vertexUpdate);
        void DuplicateBatch(int entityID, int newID);

        IBatch GetBatch(int entityID);
        IBatch GetBatchOrDefault(int entityID);
        IEnumerable<IEntity> GetEntitiesForBatch(IBatch batch);

        IEnumerable<IBatch> GetBatches(RenderTypes renderType);
        IEnumerable<IBatch> GetBatches(RenderTypes renderType, IEnumerable<int> entityIDs);
        IEnumerable<IBatch> GetBatchesInOrder(RenderTypes renderType, IEnumerable<int> entityIDs);
        //void DrawBatches(RenderTypes renderType);
    }
}
