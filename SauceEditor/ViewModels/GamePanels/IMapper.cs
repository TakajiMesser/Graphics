using SpiceEngineCore.Maps;

namespace SauceEditor.ViewModels
{
    // TODO - Get a better name
    public interface IMapper
    {
        void AddMapBrush(MapBrush mapBrush);
        //void AddMapActor(MapActor mapActor);
        void AddMapVolume(MapVolume mapVolume);
        void AddMapLight(MapLight mapLight);
    }
}