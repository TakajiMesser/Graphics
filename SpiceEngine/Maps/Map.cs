using SpiceEngineCore.Maps;
using SpiceEngineCore.Serialization.Converters;
using System.Collections.Generic;

namespace SpiceEngine.Maps
{
    /// <summary>
    /// A map should consist of a collection of static brushes, actors, a camera, and/or a player (cutscenes and menu's won't have a player)
    /// 
    /// </summary>
    public abstract class Map
    {
        public MapCamera Camera { get; set; }
        public List<MapActor> Actors { get; set; } = new List<MapActor>();
        public List<MapBrush> Brushes { get; set; } = new List<MapBrush>();
        public List<MapVolume> Volumes { get; set; } = new List<MapVolume>();
        public List<MapLight> Lights { get; set; } = new List<MapLight>();
        public List<string> SkyboxTextureFilePaths { get; set; } = new List<string>();

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
