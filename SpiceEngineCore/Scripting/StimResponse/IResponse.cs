using System;

namespace SpiceEngineCore.Scripting.StimResponse
{
    public interface IResponse
    {
        Stimulus Stimulus { get; set; }

        event EventHandler<StimulusTriggeredEventArgs> Triggered;

        //virtual void Tick(BehaviorContext context)
    }
}
