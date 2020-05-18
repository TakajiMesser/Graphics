using SpiceEngineCore.Scripting;
using System.Collections.Generic;

namespace SpiceEngineCore.Components.Builders
{
    public interface IBehaviorBuilder : IComponentBuilder<IBehavior>
    {
        IEnumerable<IScript> GetScripts();
        IEnumerable<IStimulus> GetStimuli();
        IEnumerable<IProperty> GetProperties();
    }
}
