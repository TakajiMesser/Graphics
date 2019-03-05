using SpiceEngine.Scripting.StimResponse;
using System.Collections.Generic;

namespace SpiceEngine.Scripting
{
    public interface IStimulusProvider
    {
        IEnumerable<Stimulus> GetStimuli(int entityID);
    }
}
