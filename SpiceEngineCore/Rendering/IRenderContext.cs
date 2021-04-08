using System;

namespace SpiceEngineCore.Rendering
{
    public interface IRenderContext : IDisposable
    {
        bool IsDisposed { get; }

        void MakeCurrent();
        void Update();
    }
}
