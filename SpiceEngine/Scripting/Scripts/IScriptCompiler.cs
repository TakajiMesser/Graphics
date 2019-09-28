using System.Collections.Generic;

namespace SpiceEngine.Scripting.Scripts
{
    public interface IScriptCompiler
    {
        void AddScript(Script script);
        void AddScripts(IEnumerable<Script> scripts);
        void CompileScripts();
        void ClearScripts();
    }
}
