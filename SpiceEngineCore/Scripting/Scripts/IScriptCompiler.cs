using System.Collections.Generic;

namespace SpiceEngineCore.Scripting.Scripts
{
    public interface IScriptCompiler
    {
        void AddScript(Script script);
        void AddScripts(IEnumerable<Script> scripts);
        void CompileScripts();
        void ClearScripts();
    }
}
