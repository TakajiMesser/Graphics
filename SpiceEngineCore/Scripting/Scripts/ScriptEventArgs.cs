using System;

namespace SpiceEngineCore.Scripting.Scripts
{
    public class ScriptEventArgs : EventArgs
    {
        public Script Script { get; private set; }

        public ScriptEventArgs(Script script)
        {
            Script = script;
        }

        /*public string Name { get; set; }
        public string SourcePath { get; set; }

        [JsonIgnore]
        public Type ExportedType { get; set; }

        [JsonIgnore]
        public List<CompilerError> Errors { get; private set; } = new List<CompilerError>();

        [JsonIgnore]
        public bool HasErrors => Errors.Count > 0;

        [JsonIgnore]
        public bool IsCompiled => ExportedType != null || HasErrors;

        public string GetContent() => File.ReadAllText(SourcePath);*/
    }
}
