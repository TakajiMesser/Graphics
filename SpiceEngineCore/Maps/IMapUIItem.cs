using SpiceEngineCore.Entities.UserInterfaces;
using SpiceEngineCore.Game.Loading.Builders;

namespace SpiceEngineCore.Maps
{
    public interface IMapUIItem : IMapEntity, IRenderableBuilder
    {
        void UpdateFrom(IUIItem uiItem);
    }
}
