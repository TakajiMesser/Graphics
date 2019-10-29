using SpiceEngineCore.Entities;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Maps;
using System;
using System.Threading.Tasks;

namespace SpiceEngineCore.Game.Loading
{
    public interface IGameLoader
    {
        EntityMap EntityMapping { get; }
        bool TrackEntityMapping { get; set; }
        int RendererWaitCount { get; set; }
        bool IsLoading { get; }

        event EventHandler<EventArgs> TimedOut;
        event EventHandler<EntityMapEventArgs> EntitiesMapped;

        void SetEntityProvider(IEntityProvider entityProvider);
        void SetPhysicsLoader(IEntityLoader<IShapeBuilder> physicsLoader);
        void SetBehaviorLoader(IEntityLoader<IBehaviorBuilder> behaviorLoader);
        void AddRenderableLoader(IEntityLoader<IRenderableBuilder> renderableLoader);

        void Add(IEntityBuilder entityBuilder, IShapeBuilder shapeBuilder, IBehaviorBuilder behaviorBuilder, IRenderableBuilder renderableBuilder);
        void AddFromMapEntity(MapBrush mapBrush);

        Task LoadAsync();
        void Load();
    }
}
