using System.Collections.Generic;

namespace SpiceEngineCore.Scripting
{
    public interface IStimulusProvider
    {
        IEnumerable<IStimulus> GetStimuli(int entityID);
    }
}
