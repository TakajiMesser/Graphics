using System;
using System.Threading.Tasks;

namespace SpiceEngineCore.Rendering
{
    public interface IInvoker
    {
        Task InvokeAsync(Action action);
        void InvokeSync(Action action);

        void ForceUpdate();
    }
}
