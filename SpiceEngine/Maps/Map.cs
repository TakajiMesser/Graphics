using SpiceEngineCore.Maps;
using SpiceEngineCore.Serialization.Converters;
using System.Collections.Generic;

namespace SpiceEngine.Maps
{
    /// <summary>
    /// A map should consist of a collection of static brushes, actors, a camera, and/or a player (cutscenes and menu's won't have a player)
    /// 
    /// </summary>
    public abstract class Map : IMap
    {
        public List<MapCamera> Cameras { get; set; } = new List<MapCamera>();
        public List<MapActor> Actors { get; set; } = new List<MapActor>();
        public List<MapBrush> Brushes { get; set; } = new List<MapBrush>();
        public List<MapVolume> Volumes { get; set; } = new List<MapVolume>();
        public List<MapLight> Lights { get; set; } = new List<MapLight>();

        public List<string> SkyboxTextureFilePaths { get; set; } = new List<string>();

        public int CameraCount => Cameras.Count;
        public int ActorCount => Actors.Count;
        public int BrushCount => Brushes.Count;
        public int VolumeCount => Volumes.Count;
        public int LightCount => Lights.Count;

        public IMapCamera GetCameraAt(int index) => Cameras[index];
        public IMapActor GetActorAt(int index) => Actors[index];
        public IMapBrush GetBrushAt(int index) => Brushes[index];
        public IMapVolume GetVolumeAt(int index) => Volumes[index];
        public IMapLight GetLightAt(int index) => Lights[index];

        public void AddCamera(IMapCamera mapCamera) { }
        public void AddActor(IMapActor mapActor) { }
        public void AddBrush(IMapBrush mapBrush) { }
        public void AddVolume(IMapVolume mapVolume) { }
        public void AddLight(IMapLight mapLight) { }

        protected abstract void CalculateBounds();

        public void Save(string filePath) => Serializer.Save(filePath, this as Map3D);

        public static Map Load(string filePath)
        {
            var map = Serializer.Load<Map3D>(filePath);
            map.CalculateBounds();

            return map;
        }
    }
}
