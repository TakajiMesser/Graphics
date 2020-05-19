using SpiceEngineCore.Scripting;
using System;

namespace UmamiScriptingCore.Behaviors.StimResponse
{
    public class StimulusTriggeredEventArgs : EventArgs
    {
        public StimulusTriggeredEventArgs(IStimulus stimulus) => Stimulus = stimulus;

        public IStimulus Stimulus { get; private set; }
    }
}
