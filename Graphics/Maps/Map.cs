using Graphics.GameObjects;
using Graphics.Helpers;
using Graphics.Physics.Collision;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Graphics.Maps
{
    /// <summary>
    /// A map should consist of a collection of static brushes, gameObjects, a camera, and/or a player (cutscenes and menu's won't have a player)
    /// 
    /// </summary>
    public class Map
    {
        public Quad Boundaries { get; private set; }
        public MapPlayer Player { get; set; }
        public MapCamera Camera { get; set; }
        public List<MapGameObject> GameObjects { get; set; } = new List<MapGameObject>();
        public List<MapBrush> Brushes { get; set; } = new List<MapBrush>();

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
