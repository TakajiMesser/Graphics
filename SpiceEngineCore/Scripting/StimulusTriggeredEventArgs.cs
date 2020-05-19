using System;

namespace SpiceEngineCore.Scripting
{
    public class StimulusTriggeredEventArgs : EventArgs
    {
        public IStimulus Stimulus { get; private set; }

        public StimulusTriggeredEventArgs(IStimulus stimulus) => Stimulus = stimulus;
    }
}
