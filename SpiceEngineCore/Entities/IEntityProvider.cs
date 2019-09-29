using SpiceEngineCore.Game.Loading;
using System;
using System.Collections.Generic;

namespace SpiceEngineCore.Entities
{
    public interface IEntityProvider
    {
        IEnumerable<int> EntityRenderIDs { get; }
        IEnumerable<int> EntityScriptIDs { get; }
        IEnumerable<int> EntityPhysicsIDs { get; }
        IEnumerable<int> EntitySelectIDs { get; }

        List<ILight> Lights { get; }
        List<IActor> Actors { get; }

        event EventHandler<EntityBuilderEventArgs> EntitiesAdded;
        event EventHandler<IDEventArgs> EntitiesRemoved;

        int AddEntity(IEntityBuilder entityBuilder);
        int AddEntity(IEntity entity);
        void AddEntities(IEnumerable<IEntity> entities);

        IEnumerable<int> AssignEntityIDs(IEnumerable<IEntityBuilder> entityBuilders);
        void LoadEntity(int id);
        void Load();

        IEntity GetEntity(int id);
        IEntity GetEntityOrDefault(int id);
        IEnumerable<IEntity> GetEntities(IEnumerable<int> ids);
        IActor GetActor(string name);
        //EntityTypes GetEntityType(int id);

        void RemoveEntityByID(int id);
        IEntity DuplicateEntity(IEntity entity);

        bool ContainsLayer(string layerName);
        void AddLayer(string name);
        void AddEntitiesToLayer(string layerName, IEnumerable<int> entityIDs);
        IEnumerable<int> GetLayerEntityIDs(string layerName);
        //void SetLayerState(string name, LayerStates state);
        //void SetRenderLayerState(string name, LayerStates state);
        //void SetSelectLayerState(string name, LayerStates state);
    }
}
