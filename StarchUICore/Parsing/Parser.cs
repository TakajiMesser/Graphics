using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpiceEngineCore.Rendering.Vertices;
using StarchUICore.Attributes.Positions;
using StarchUICore.Attributes.Sizes;
using StarchUICore.Attributes.Units;
using StarchUICore.Builders;
using StarchUICore.Themes;

namespace StarchUICore.Views.Controls.Buttons
{
    public class Parser
    {
        public static IElement ParseFromFile(string filePath)
        {
            return null;
        }

        public static IElement ParseFromString(string jsonText)
        {
            var json = JObject.Parse(jsonText);
            return null;
        }
    }
}
