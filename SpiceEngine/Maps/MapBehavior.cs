using SpiceEngine.Scripting;
using SpiceEngineCore.Serialization.Converters;
using System.Collections.Generic;
using UmamiScriptingCore;
using UmamiScriptingCore.Behaviors;
using UmamiScriptingCore.Scripts;

namespace SpiceEngine.Maps
{
    public class MapBehavior
    {
        public string Name { get; set; }
        public string FilePath { get; set; }

        public MapNode RootNode { get; set; }
        public List<Response> Responses { get; set; } = new List<Response>();

        public IEnumerable<Script> GetScripts() => RootNode?.GetScripts();

        public IBehavior ToBehavior(int entityID)
        {
            var behavior = new Behavior(entityID);

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
