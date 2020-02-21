using SpiceEngineCore.Scripting;
using System.Collections.Generic;

namespace SpiceEngineCore.Game.Loading.Builders
{
    public interface IBehaviorBuilder : IComponentBuilder<IBehavior>
    {
        IEnumerable<IScript> Scripts { get; }
        IEnumerable<IStimulus> Stimuli { get; }
        IEnumerable<IProperty> Properties { get; }
    }
}
