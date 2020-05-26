using OpenTK;

namespace SpiceEngineCore.Game.Settings
{
    public class Configuration
    {
        public VSyncMode VSyncMode { get; set; }
        public int UpdatesPerSecond { get; set; }
        public int RendersPerSecond { get; set; }
    }
}
