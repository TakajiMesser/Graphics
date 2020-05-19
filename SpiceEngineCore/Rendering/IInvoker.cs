using System;
using System.Threading.Tasks;

namespace SpiceEngineCore.Rendering
{
    public interface IInvoker
    {
        Task RunAsync(Action action);
        void RunSync(Action action);

        void ForceUpdate();
    }
}
