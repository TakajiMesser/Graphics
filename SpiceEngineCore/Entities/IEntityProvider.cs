using SpiceEngineCore.Entities.Actors;
using SpiceEngineCore.Entities.Brushes;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Entities.Layers;
using SpiceEngineCore.Entities.UserInterfaces;
using SpiceEngineCore.Game.Loading;
using SpiceEngineCore.Game.Loading.Builders;
using System;
using System.Collections.Generic;

namespace SpiceEngineCore.Entities
{
    public interface IEntityProvider
    {
        ILayerProvider LayerProvider { get; }
        ICamera ActiveCamera { get; }

        List<ICamera> Cameras { get; }
        List<IActor> Actors { get; }
        List<IBrush> Brushes { get; }
        List<IVolume> Volumes { get; }
        List<ILight> Lights { get; }
        List<IUIItem> UIItems { get; }

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
