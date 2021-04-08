using System;

namespace SpiceEngineCore.HID
{
    public interface IWindowContext : IDisposable
    {
        bool IsDisposed { get; }

        void MakeCurrent();
        void SwapBuffers();
    }
}
