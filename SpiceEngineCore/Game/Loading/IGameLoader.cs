using SpiceEngineCore.Entities;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Maps;
using System;
using System.Threading.Tasks;
using EntityMappingEventArgs = SpiceEngineCore.Maps.EntityMappingEventArgs;

namespace SpiceEngineCore.Game.Loading
{
    public interface IGameLoader
    {
        EntityMapping EntityMapping { get; }
        bool TrackEntityMapping { get; set; }
        int RendererWaitCount { get; set; }
        bool IsLoading { get; }

        event EventHandler<EventArgs> TimedOut;
        event EventHandler<EntityMappingEventArgs> EntitiesMapped;

        void SetEntityProvider(IEntityProvider entityProvider);
        void SetPhysicsLoader(IComponentLoader<IShapeBuilder> physicsLoader);
        void SetBehaviorLoader(IComponentLoader<IBehaviorBuilder> behaviorLoader);
        void AddRenderableLoader(IComponentLoader<IRenderableBuilder> renderableLoader);

        void Add(IEntityBuilder entityBuilder, IShapeBuilder shapeBuilder, IBehaviorBuilder behaviorBuilder, IRenderableBuilder renderableBuilder);
        void Add(IMapEntity3D mapEntity);
        void AddFromMap(IMap map);

        Task LoadAsync();
        void Load();
    }
}
