using SpiceEngineCore.Scripting;
using System;

namespace UmamiScriptingCore.Behaviors.StimResponse
{
    public interface IResponse
    {
        IStimulus Stimulus { get; set; }

        event EventHandler<StimulusTriggeredEventArgs> Triggered;

        void Tick(BehaviorContext context);
    }
}
