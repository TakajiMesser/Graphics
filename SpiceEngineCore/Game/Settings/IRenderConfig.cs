using SpiceEngineCore.Rendering;

namespace SpiceEngineCore.Game.Settings
{
    public interface IRenderConfig
    {
        string API { get; }
        int MajorVersion { get; }
        int MinorVersion { get; }
    }
}
