using System;

namespace SpiceEngineCore.Scripting.StimResponse
{
    public class StimulusTriggeredEventArgs : EventArgs
    {
        public Stimulus Stimulus { get; private set; }

        public StimulusTriggeredEventArgs(Stimulus stimulus) => Stimulus = stimulus;
    }
}
