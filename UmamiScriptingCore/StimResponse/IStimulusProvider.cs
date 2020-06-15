using System.Collections.Generic;

namespace UmamiScriptingCore.StimResponse
{
    public interface IStimulusProvider
    {
        IEnumerable<IStimulus> GetStimuli(int entityID);
    }
}
