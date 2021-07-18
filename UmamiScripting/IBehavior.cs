using SpiceEngineCore.Commands;
using SpiceEngineCore.Components;
using SpiceEngineCore.Game;
using UmamiScriptingCore.Behaviors;
using UmamiScriptingCore.StimResponse;

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
        void SetCommander(ICommander commander);
        void SetStimulusProvider(IStimulusProvider stimulusProvider);

        void SetProperty(string name, object value);
    }
}
