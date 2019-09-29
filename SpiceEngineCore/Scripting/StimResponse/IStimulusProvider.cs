using System.Collections.Generic;

namespace SpiceEngineCore.Scripting.StimResponse
{
    public interface IStimulusProvider
    {
        IEnumerable<Stimulus> GetStimuli(int entityID);
    }
}
