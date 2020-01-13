namespace SpiceEngineCore.Maps
{
    public interface IMap
    {
        int CameraCount { get; }
        int BrushCount { get; }
        int ActorCount { get; }
        int LightCount { get; }
        int VolumeCount { get; }
        int UIElementCount { get; }

        IMapCamera GetCameraAt(int index);
        IMapBrush GetBrushAt(int index);
        IMapActor GetActorAt(int index);
        IMapLight GetLightAt(int index);
        IMapVolume GetVolumeAt(int index);
        IMapUIElement GetUIElementAt(int index);

        int AddCamera(IMapCamera mapCamera);
        int AddBrush(IMapBrush mapBrush);
        int AddActor(IMapActor mapActor);
        int AddLight(IMapLight mapLight);
        int AddVolume(IMapVolume mapVolume);
        int AddUIElement(IMapUIElement mapUIElement);
    }
}
