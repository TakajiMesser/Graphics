using System;
using System.Threading.Tasks;

namespace SpiceEngine.Rendering
{
    public interface IInvoker
    {
        void Run(Action action);
        Task RunAsync(Action action);
    }
}
