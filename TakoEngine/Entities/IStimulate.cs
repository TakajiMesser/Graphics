using OpenTK;
using System.Collections.Generic;
using TakoEngine.Scripting.StimResponse;

namespace TakoEngine.Entities
{
    public interface IStimulate
    {
        List<Stimulus> Stimuli { get; }
    }
}
