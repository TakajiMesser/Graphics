using SpiceEngineCore.Entities.Actors;
using SpiceEngineCore.Game.Loading.Builders;

namespace SpiceEngineCore.Maps
{
    public interface IMapActor : IMapEntity, IRenderableBuilder, IShapeBuilder, IBehaviorBuilder, IAnimatorBuilder
    {
        void UpdateFrom(IActor actor);
    }
}
