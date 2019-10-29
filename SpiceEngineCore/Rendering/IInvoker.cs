using System;
using System.Threading.Tasks;

namespace SpiceEngineCore.Rendering
{
    public interface IInvoker
    {
        void Run(Action action);
        Task RunAsync(Action action);
    }
}
