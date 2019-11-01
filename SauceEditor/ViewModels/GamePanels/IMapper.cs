using SpiceEngineCore.Maps;

namespace SauceEditor.ViewModels
{
    // TODO - Get a better name
    public interface IMapper
    {
        void AddMapCamera(IMapCamera mapCamera);
        void AddMapBrush(IMapBrush mapBrush);
        void AddMapActor(IMapActor mapActor);
        void AddMapVolume(IMapVolume mapVolume);
        void AddMapLight(IMapLight mapLight);
    }
}