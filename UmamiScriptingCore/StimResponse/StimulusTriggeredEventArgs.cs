using System;

namespace UmamiScriptingCore.StimResponse
{
    public class StimulusTriggeredEventArgs : EventArgs
    {
        public StimulusTriggeredEventArgs(IStimulus stimulus) => Stimulus = stimulus;

        public IStimulus Stimulus { get; private set; }
    }
}
