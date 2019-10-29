using SpiceEngineCore.Scripting;
using SpiceEngineCore.Scripting.Scripts;
using SpiceEngineCore.Scripting.StimResponse;
using SpiceEngineCore.Serialization.Converters;
using System.Collections.Generic;

namespace SpiceEngineCore.Maps
{
    public class MapBehavior
    {
        public string Name { get; set; }
        public string FilePath { get; set; }

        public MapNode RootNode { get; set; }
        public List<Response> Responses { get; set; } = new List<Response>();

        public IEnumerable<Script> GetScripts() => RootNode?.GetScripts();

        public Behavior ToBehavior()
        {
            var behavior = new Behavior();

            if (RootNode != null)
            {
                var rootNode = RootNode.ToNode();
                behavior.PushRootNode(rootNode);
            }

            foreach (var response in Responses)
            {
                behavior.AddResponse(response);
            }

            return behavior;
        }

        public void Save(string filePath) => Serializer.Save(filePath, this as MapBehavior);

        public static MapBehavior Load(string filePath) => Serializer.Load<MapBehavior>(filePath);
    }
}
