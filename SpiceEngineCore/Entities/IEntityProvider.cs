using SpiceEngineCore.Entities.Layers;
using SpiceEngineCore.Game.Loading;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Scenes;
using System;
using System.Collections.Generic;

namespace SpiceEngineCore.Entities
{
    public interface IEntityProvider
    {
        ILayerProvider LayerProvider { get; }
        IScene ActiveScene { get; }

        event EventHandler<EntityBuilderEventArgs> EntitiesAdded;
        event EventHandler<IDEventArgs> EntitiesRemoved;

        int AddEntity(IEntityBuilder entityBuilder);
        int AddEntity(IEntity entity);
        void AddEntities(IEnumerable<IEntity> entities);

        IEnumerable<int> AssignEntityIDs(IEnumerable<IEntityBuilder> entityBuilders);
        void LoadEntity(int id);
        void Load();

        IEntity GetEntity(int id);
        INamedEntity GetEntity(string name);
        IEntity GetEntityOrDefault(int id);
        IEnumerable<IEntity> GetEntities(IEnumerable<int> ids);

        void RemoveEntityByID(int id);
        IEntity DuplicateEntity(IEntity entity);
    }
}
