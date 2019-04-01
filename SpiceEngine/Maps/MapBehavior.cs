using SpiceEngine.Scripting.Behaviors;
using SpiceEngine.Scripting.StimResponse;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;

namespace SpiceEngine.Maps
{
    public class MapBehavior
    {
        public string Name { get; set; }
        public string FilePath { get; set; }

        public MapNode RootNode { get; set; }
        public List<Response> Responses { get; set; } = new List<Response>();

        public Behavior ToBehavior()
        {
            var behavior = new Behavior();

            if (RootNode != null)
            {
                behavior.PushRootNode(RootNode.ToNode());
            }

            foreach (var response in Responses)
            {
                behavior.AddResponse(response);
            }

            return behavior;
        }

        public void Save(string path)
        {
            using (var writer = XmlWriter.Create(path))
            {
                var serializer = new NetDataContractSerializer();
                serializer.WriteObject(writer, this);
            }
        }

        public static MapBehavior Load(string path)
        {
            using (var reader = XmlReader.Create(path))
            {
                var serializer = new NetDataContractSerializer();
                return serializer.ReadObject(reader, true) as MapBehavior;
            }
        }
    }
}
