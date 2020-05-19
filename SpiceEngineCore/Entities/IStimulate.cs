using SpiceEngineCore.Scripting;
using System.Collections.Generic;

namespace SpiceEngineCore.Entities
{
    public interface IStimulate
    {
        List<IStimulus> Stimuli { get; }
    }
}
