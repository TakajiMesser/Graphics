using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace UmamiScriptingCore.Scripts
{
    /// <summary>
    /// A script is a set of sequential commands
    /// Each command can be something this object needs to communicate to another object, or performed by itself
    /// Each command either needs to be associated with 
    /// </summary>
    public class Script : IScript
    {
        public string Name { get; set; }
        public string Contents { get; set; }

        [JsonIgnore]
        public Type ExportedType { get; set; }

        // TODO - Determine how we want to represent errors in the core library
        [JsonIgnore]
        public List<string> Errors { get; private set; } = new List<string>();
        //public List<CompilerError> Errors { get; private set; } = new List<CompilerError>();

        [JsonIgnore]
        public bool HasErrors => Errors.Count > 0;

        [JsonIgnore]
        public bool IsCompiled => ExportedType != null || HasErrors;

        public event EventHandler<ScriptEventArgs> Compiled;

        public void OnCompiled(object sender, ScriptEventArgs args) => Compiled?.Invoke(sender, args);
    }
}
