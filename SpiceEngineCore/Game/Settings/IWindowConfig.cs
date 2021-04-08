using SpiceEngineCore.Rendering;

namespace SpiceEngineCore.Game.Settings
{
    /*public enum WindowTypes
    {
        Native,
        Frame,
        Hidden
    }*/

    public interface IWindowConfig
    {
        string Title { get; }
        //WindowTypes WindowType { get; }
        Resolution Size { get; }

        int UpdatesPerSecond { get; }
        int RendersPerSecond { get; }

        bool Visible { get; }
        bool HandleInputEvents { get; }
        bool VSync { get; }
    }
}
