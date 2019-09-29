using SpiceEngineCore.Scripting.StimResponse;
using System.Collections.Generic;

namespace SpiceEngine.Entities
{
    public interface IStimulate
    {
        List<Stimulus> Stimuli { get; }
    }
}
