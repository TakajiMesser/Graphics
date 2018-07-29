using OpenTK;
using System.Collections.Generic;
using SpiceEngine.Scripting.StimResponse;

namespace SpiceEngine.Entities
{
    public interface IStimulate
    {
        List<Stimulus> Stimuli { get; }
    }
}
