using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakoEngine.Scripting.StimResponse
{
    public class StimulusReceivedEventArgs : EventArgs
    {
        public Stimulus Stimulus { get; private set; }

        public StimulusReceivedEventArgs(Stimulus stimulus)
        {
            Stimulus = stimulus;
        }
    }
}
