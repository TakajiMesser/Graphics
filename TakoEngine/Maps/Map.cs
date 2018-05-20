using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public MapCamera Camera { get; set; }
        public List<MapActor> Actors { get; set; } = new List<MapActor>();
        public List<MapBrush> Brushes { get; set; } = new List<MapBrush>();
        public List<MapVolume> Volumes { get; set; } = new List<MapVolume>();
        public List<Light> Lights { get; set; } = new List<Light>();
        public List<string> SkyboxTextureFilePaths { get; set; } = new List<string>();

        [IgnoreDataMember]
        public Quad Boundaries { get; private set; }

        public Map() { }

        private void CalculateBounds()
        {
            Boundaries = new Quad()
            {
                Min = new Vector3
                {
                    X = new float[]
                    {
                        Actors.Min(a => a.Position.X),
                        Brushes.Min(b => b.Position.X),
                        //Volumes.Min(v => v.Position.X),
                        Lights.Min(l => l.Position.X)
                    }.Min(),
                    Y = new float[]
                    {
                        Actors.Min(a => a.Position.Y),
                        Brushes.Min(b => b.Position.Y),
                        //Volumes.Min(v => v.Position.Y),
                        Lights.Min(l => l.Position.Y)
                    }.Min(),
                    Z = new float[]
                    {
                        Actors.Min(a => a.Position.Z),
                        Brushes.Min(b => b.Position.Z),
                        //Volumes.Min(v => v.Position.Z),
                        Lights.Min(l => l.Position.Z)
                    }.Min()
                },
                Max = new Vector3
                {
                    X = new float[]
                    {
                        Actors.Max(a => a.Position.X),
                        Brushes.Max(b => b.Position.X),
                        //Volumes.Max(v => v.Position.X),
                        Lights.Max(l => l.Position.X)
                    }.Max(),
                    Y = new float[]
                    {
                        Actors.Max(a => a.Position.Y),
                        Brushes.Max(b => b.Position.Y),
                        //Volumes.Max(v => v.Position.Y),
                        Lights.Max(l => l.Position.Y)
                    }.Max(),
                    Z = new float[]
                    {
                        Actors.Max(a => a.Position.Z),
                        Brushes.Max(b => b.Position.Z),
                        //Volumes.Max(v => v.Position.Z),
                        Lights.Max(l => l.Position.Z)
                    }.Max()
                }
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
                var map = serializer.ReadObject(reader, true) as Map;

                map.CalculateBounds();
                return map;
            }
        }
    }
}
