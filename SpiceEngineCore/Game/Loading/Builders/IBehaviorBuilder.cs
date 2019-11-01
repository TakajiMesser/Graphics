using SpiceEngineCore.Scripting;
using SpiceEngineCore.Scripting.Properties;
using SpiceEngineCore.Scripting.Scripts;
using SpiceEngineCore.Scripting.StimResponse;
using System.Collections.Generic;

namespace SpiceEngineCore.Game.Loading.Builders
{
    public interface IBehaviorBuilder : IComponentBuilder<IBehavior>
    {
        IEnumerable<Script> Scripts { get; }
        List<Stimulus> Stimuli { get; }
        List<Property> Properties { get; }

        IBehavior ToBehavior();
    }
}
