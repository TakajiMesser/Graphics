using SpiceEngineCore.Game.Settings;
using System;

namespace SpiceEngineCore.HID
{
    public interface IWindowContextFactory
    {
        IWindowContext CreateWindowContext(IWindowConfig configuration);
        IntPtr CreateWindowHandle(IWindowConfig configuration);
    }
}
