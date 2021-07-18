using SpiceEngineCore.Entities;
using SpiceEngineCore.Maps;
using SpiceEngineCore.Rendering;
using System;
using System.Threading.Tasks;
using EntityMappingEventArgs = SpiceEngineCore.Maps.EntityMappingEventArgs;

namespace SpiceEngineCore.Game.Loading
{
    public interface IGameLoader
    {
        bool IsInEditorMode { get; set; }

        EntityMapping EntityMapping { get; }
        bool TrackEntityMapping { get; set; }
        bool IsLoading { get; }

        event EventHandler<EventArgs> TimedOut;
        event EventHandler<EntityMappingEventArgs> EntitiesMapped;

        void SetEntityProvider(IEntityProvider entityProvider);

        void AddComponentLoader(IComponentLoader componentLoader);
        void AddRenderableLoader(IRenderableLoader renderableLoader);

        //void Add(IEntityBuilder entityBuilder, IShapeBuilder shapeBuilder, IBehaviorBuilder behaviorBuilder, IRenderableBuilder renderableBuilder);
        void Add(IMapEntity mapEntity);
        void AddFromMap(IMap map);

        Task LoadAsync();
        void LoadSync();
    }
}
