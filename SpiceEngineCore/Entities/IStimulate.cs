using SpiceEngineCore.Scripting.StimResponse;
using System.Collections.Generic;

namespace SpiceEngineCore.Entities
{
    public interface IStimulate
    {
        List<Stimulus> Stimuli { get; }
    }
}
