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
        public List<MapBrush> Brushes { get; set; } = new List<MapBrush>();
        public List<MapActor> Actors { get; set; } = new List<MapActor>();
        public List<MapLight> Lights { get; set; } = new List<MapLight>();
        public List<MapVolume> Volumes { get; set; } = new List<MapVolume>();

        public List<string> SkyboxTextureFilePaths { get; set; } = new List<string>();

        public int CameraCount => Cameras.Count;
        public int BrushCount => Brushes.Count;
        public int ActorCount => Actors.Count;
        public int LightCount => Lights.Count;
        public int VolumeCount => Volumes.Count;

        public IMapCamera GetCameraAt(int index) => Cameras[index];
        public IMapBrush GetBrushAt(int index) => Brushes[index];
        public IMapActor GetActorAt(int index) => Actors[index];
        public IMapLight GetLightAt(int index) => Lights[index];
        public IMapVolume GetVolumeAt(int index) => Volumes[index];

        public int AddCamera(IMapCamera mapCamera)
        {
            if (mapCamera is MapCamera camera)
            {
                Cameras.Add(camera);
                return Cameras.Count - 1;
            }

            return -1;
        }

        public int AddBrush(IMapBrush mapBrush)
        {
            if (mapBrush is MapBrush brush)
            {
                Brushes.Add(brush);
                return Brushes.Count - 1;
            }

            return -1;
        }

        public int AddActor(IMapActor mapActor)
        {
            if (mapActor is MapActor actor)
            {
                Actors.Add(actor);
                return Actors.Count - 1;
            }

            return -1;
        }

        public int AddLight(IMapLight mapLight)
        {
            if (mapLight is MapLight light)
            {
                Lights.Add(light);
                return Lights.Count - 1;
            }

            return -1;
        }

        public int AddVolume(IMapVolume mapVolume)
        {
            if (mapVolume is MapVolume volume)
            {
                Volumes.Add(volume);
                return Volumes.Count - 1;
            }

            return -1;
        }

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
