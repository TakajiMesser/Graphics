using SpiceEngineCore.Rendering;

namespace SpiceEngineCore.Game.Settings
{
    public class Configuration
    {
        public Resolution WindowSize { get; set; } = new Resolution(1280, 720);
        public string WindowTitle { get; set; } = "My First OpenGL Game";

        public int UpdatesPerSecond { get; set; } = 60;
        public int RendersPerSecond { get; set; } = 60;

        public bool VSync { get; set; } = false;
    }
}
