using SpiceEngine.Maps;

namespace SauceEditor.Models.Components
{
    public class MapComponent : Component
    {
        public Map Map { get; set; }

        public override void Save() => Map.Save(Path);
        public override void Load() => Map = Map.Load(Path);
    }
}
