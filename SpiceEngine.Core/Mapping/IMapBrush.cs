using SpiceEngineCore.Entities.Brushes;
using SpiceEngineCore.Game.Loading.Builders;

namespace SpiceEngineCore.Maps
{
    public interface IMapBrush : IMapEntity, IRenderableBuilder//, IShapeBuilder
    {
        void UpdateFrom(IBrush brush);
    }
}
