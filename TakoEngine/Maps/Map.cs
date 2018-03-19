using OpenTK;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using TakoEngine.Entities.Lights;
using TakoEngine.Physics.Collision;

namespace TakoEngine.Maps
{
    /// <summary>
    /// A map should consist of a collection of static brushes, actors, a camera, and/or a player (cutscenes and menu's won't have a player)
    /// 
    /// </summary>
    public class Map
    {
        public Quad Boundaries { get; private set; }
        public MapCamera Camera { get; set; }
        public List<MapActor> Actors { get; set; } = new List<MapActor>();
        public List<MapBrush> Brushes { get; set; } = new List<MapBrush>();
        public List<Light> Lights { get; set; } = new List<Light>();
        public List<string> SkyboxTextureFilePaths { get; set; }

        public Map()
        {
            Boundaries = new Quad()
            {
                Min = new Vector3(-100.0f, -100.0f, 0.0f),
                Max = new Vector3(100.0f, 100.0f, 0.0f)
            };
        }

        public void Save(string path)
        {
            using (var writer = XmlWriter.Create(path))
            {
                var serializer = new NetDataContractSerializer();
                serializer.WriteObject(writer, this);
            }
        }

        public static Map Load(string path)
        {
            using (var reader = XmlReader.Create(path))
            {
                var serializer = new NetDataContractSerializer();
                return serializer.ReadObject(reader, true) as Map;
            }
        }
    }
}
