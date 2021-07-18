using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Batches;
using SpiceEngineCore.Rendering.Vertices;
using System;

namespace SweetGraphicsCore.Rendering.Batches
{
    public interface IBatcher
    {
        bool IsLoaded { get; }

        IBatch GetBatch(int entityID);
        IBatch GetBatchOrDefault(int entityID);

        void AddEntity(int entityID, IRenderable renderable);
        void RemoveByEntityID(int entityID);

        void Load(IRenderContext renderContext);
        //void Load(int entityID);

        void UpdateVertices(int entityID, Func<IVertex, IVertex> vertexUpdate);
        void DuplicateBatch(int entityID, int newID);

        IBatchAction CreateBatchAction();
    }
}
