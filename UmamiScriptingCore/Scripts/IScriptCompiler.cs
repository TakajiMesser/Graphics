using SpiceEngineCore.Scripting;
using System.Collections.Generic;

namespace UmamiScriptingCore.Scripts
{
    public interface IScriptCompiler
    {
        void AddScript(IScript script);
        void AddScripts(IEnumerable<IScript> scripts);
        void CompileScripts();
        void ClearScripts();
    }
}
