using System;

namespace SpiceEngineCore.Rendering
{
    public interface IRenderContext : IDisposable
    {
        bool IsDisposed { get; }

        //void Initialize();
        void MakeCurrent();
        void Update();
        void SwapBuffers();
    }
}
