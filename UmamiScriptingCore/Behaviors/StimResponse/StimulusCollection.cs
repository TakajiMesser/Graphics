using SpiceEngineCore.Scripting;
using System.Collections.Generic;

namespace UmamiScriptingCore.Behaviors.StimResponse
{
    public class StimulusCollection
    {
        private List<IStimulus> _stimuli = new List<IStimulus>();

        public IEnumerable<IStimulus> Stimuli => _stimuli;

        public void AddStimuli(IEnumerable<IStimulus> stimuli) => _stimuli.AddRange(stimuli);
    }
}
