namespace SpiceEngineCore.Maps
{
    public interface IMap
    {
        int CameraCount { get; }
        int ActorCount { get; }
        int BrushCount { get; }
        int VolumeCount { get; }
        int LightCount { get; }

        IMapCamera GetCameraAt(int index);
        IMapActor GetActorAt(int index);
        IMapBrush GetBrushAt(int index);
        IMapVolume GetVolumeAt(int index);
        IMapLight GetLightAt(int index);

        void AddCamera(IMapCamera mapCamera);
        void AddActor(IMapActor mapActor);
        void AddBrush(IMapBrush mapBrush);
        void AddVolume(IMapVolume mapVolume);
        void AddLight(IMapLight mapLight);
    }
}
