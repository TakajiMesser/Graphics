using SpiceEngineCore.Entities.Actors;
using SpiceEngineCore.Game.Loading.Builders;

namespace SpiceEngineCore.Maps
{
    public interface IMapActor : IMapEntity3D, IRenderableBuilder, IShapeBuilder, IBehaviorBuilder
    {
        void UpdateFrom(IActor actor);
    }
}
