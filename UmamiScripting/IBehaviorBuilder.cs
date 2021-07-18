using SpiceEngineCore.Components;
using System.Collections.Generic;
using UmamiScriptingCore.Props;
using UmamiScriptingCore.Scripts;
using UmamiScriptingCore.StimResponse;

namespace UmamiScriptingCore
{
    public interface IBehaviorBuilder : IComponentBuilder<IBehavior>
    {
        IEnumerable<IScript> GetScripts();
        IEnumerable<IStimulus> GetStimuli();
        IEnumerable<IProperty> GetProperties();
    }
}
