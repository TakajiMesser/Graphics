using SpiceEngineCore.Rendering;

namespace SpiceEngineCore.Game.Settings
{
    public class Configuration : IWindowConfig, IRenderConfig
    {
        public string Title { get; set; } = "My First OpenGL Game";
        public Resolution Size { get; set; } = new Resolution(1280, 720);

        public int UpdatesPerSecond { get; set; } = 60;
        public int RendersPerSecond { get; set; } = 60;

        public bool Visible { get; set; } = true;
        public bool HandleInputEvents { get; set; } = true;
        public bool VSync { get; set; } = false;

        public string API { get; set; } = "OpenGL";
        public int MajorVersion { get; set; } = 4;
        public int MinorVersion { get; set; } = 6;
    }
}
