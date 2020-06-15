using SpiceEngineCore.Components;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Game;
using UmamiScriptingCore.Behaviors;

namespace UmamiScriptingCore
{
    public enum BehaviorStatus
    {
        Dormant,
        Running,
        Success,
        Failure
    }

    public interface IBehavior : IComponent
    {
        BehaviorContext Context { get; }

        BehaviorStatus Tick();

        void SetSystemProvider(ISystemProvider systemProvider);
        void SetCamera(ICamera camera);
        void SetProperty(string name, object value);
    }
}
