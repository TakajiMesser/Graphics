using SpiceEngine.Entities.Actors;
using SpiceEngine.Entities.Layers;
using SpiceEngine.Entities.Lights;
using System.Collections.Generic;

namespace SpiceEngine.Entities
{
    public interface IEntityProvider
    {
        IEnumerable<int> EntityRenderIDs { get; }
        IEnumerable<int> EntityScriptIDs { get; }
        IEnumerable<int> EntityPhysicsIDs { get; }
        IEnumerable<int> EntitySelectIDs { get; }

        int AddEntity(IEntity entity);
        IEntity GetEntity(int id);
        IEntity GetEntityOrDefault(int id);
        IEnumerable<IEntity> GetEntities(IEnumerable<int> ids);
        Actor GetActor(string name);
        EntityTypes GetEntityType(int id);

        List<ILight> Lights { get; }
        List<Actor> Actors { get; }

        void RemoveEntityByID(int id);
        IEntity DuplicateEntity(IEntity entity);

        bool ContainsLayer(string layerName);
        void AddLayer(string name);
        void AddEntitiesToLayer(string layerName, IEnumerable<int> entityIDs);
        IEnumerable<int> GetLayerEntityIDs(string layerName);
        void SetLayerState(string name, LayerStates state);
        void SetRenderLayerState(string name, LayerStates state);
        void SetSelectLayerState(string name, LayerStates state);
    }
}
