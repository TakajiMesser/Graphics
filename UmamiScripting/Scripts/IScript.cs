using System;
using System.Collections.Generic;

namespace UmamiScriptingCore.Scripts
{
    public interface IScript
    {
        string Name { get; }
        string Contents { get; }
        Type ExportedType { get; set; }
        List<string> Errors { get; }
        bool HasErrors { get; }
        bool IsCompiled { get; }

        event EventHandler<ScriptEventArgs> Compiled;

        void OnCompiled(object sender, ScriptEventArgs args);
    }
}
