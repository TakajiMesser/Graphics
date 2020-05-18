using SpiceEngineCore.Components.Animations;
using SpiceEngineCore.Components.Builders;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Maps;
using SpiceEngineCore.Physics;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Scripting;
using SpiceEngineCore.UserInterfaces;
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
        int RendererWaitCount { get; set; }
        bool IsLoading { get; }

        event EventHandler<EventArgs> TimedOut;
        event EventHandler<EntityMappingEventArgs> EntitiesMapped;

        void SetEntityProvider(IEntityProvider entityProvider);
        void SetPhysicsLoader(IComponentLoader<IShape, IShapeBuilder> physicsLoader);
        void SetBehaviorLoader(IComponentLoader<IBehavior, IBehaviorBuilder> behaviorLoader);
        void SetAnimatorLoader(IComponentLoader<IAnimator, IAnimatorBuilder> animatorLoader);
        void SetUILoader(IComponentLoader<IUIElement, IUIElementBuilder> uiLoader);
        void AddRenderableLoader(IComponentLoader<IRenderable, IRenderableBuilder> renderableLoader);

        //void Add(IEntityBuilder entityBuilder, IShapeBuilder shapeBuilder, IBehaviorBuilder behaviorBuilder, IRenderableBuilder renderableBuilder);
        void Add(IMapEntity mapEntity);
        void AddFromMap(IMap map);

        Task LoadAsync();
        void LoadSync();
    }
}
