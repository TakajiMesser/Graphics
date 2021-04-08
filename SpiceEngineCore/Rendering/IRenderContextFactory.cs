using SpiceEngineCore.Game.Settings;
using SpiceEngineCore.HID;

namespace SpiceEngineCore.Rendering
{
    public interface IRenderContextFactory
    {
        IRenderContext CreateRenderContext(IRenderConfig configuration, IWindowContext windowContext);
    }
}
