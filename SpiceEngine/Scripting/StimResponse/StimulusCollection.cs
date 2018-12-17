using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceEngine.Scripting.StimResponse
{
    public class StimulusCollection
    {
        private List<Stimulus> _stimuli = new List<Stimulus>();

        public IEnumerable<Stimulus> Stimuli => _stimuli;
        
        public void AddStimuli(IEnumerable<Stimulus> stimuli)
        {
            _stimuli.AddRange(stimuli);
        }
    }
}
