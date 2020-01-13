using SpiceEngineCore.Entities.UserInterfaces;
using SpiceEngineCore.Game.Loading.Builders;

namespace SpiceEngineCore.Maps
{
    public interface IMapUIElement : IMapEntity, IRenderableBuilder
    {
        void UpdateFrom(IUIElement uiElement);
    }
}
