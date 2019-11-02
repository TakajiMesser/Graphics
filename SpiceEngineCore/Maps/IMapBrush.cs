using SpiceEngineCore.Entities.Brushes;
using SpiceEngineCore.Game.Loading.Builders;

namespace SpiceEngineCore.Maps
{
    public interface IMapBrush : IMapEntity3D, IRenderableBuilder, IShapeBuilder
    {
        void UpdateFrom(IBrush brush);
    }
}
