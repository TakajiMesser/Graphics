using System;
using System.Collections.Generic;

namespace SpiceEngineCore.Scripting
{
    public interface IScript
    {
        string Name { get; }
        string SourcePath { get; }
        Type ExportedType { get; set; }
        List<string> Errors { get; }
        bool HasErrors { get; }
        bool IsCompiled { get; }

        event EventHandler<ScriptEventArgs> Compiled;

        string GetContent();
        void OnCompiled(object sender, ScriptEventArgs args);
    }
}
