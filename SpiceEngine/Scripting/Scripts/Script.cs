using Newtonsoft.Json;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace SpiceEngine.Scripting.Scripts
{
    /// <summary>
    /// A script is a set of sequential commands
    /// Each command can be something this object needs to communicate to another object, or performed by itself
    /// Each command either needs to be associated with 
    /// </summary>
    public class Script
    {
        public string Name { get; set; }
        public string SourcePath { get; set; }

        [JsonIgnore]
        public Type ExportedType { get; set; }

        [JsonIgnore]
        public List<CompilerError> Errors { get; private set; } = new List<CompilerError>();

        [JsonIgnore]
        public bool HasErrors => Errors.Count > 0;

        [JsonIgnore]
        public bool IsCompiled => ExportedType != null || HasErrors;

        public event EventHandler<ScriptEventArgs> Compiled;

        public string GetContent() => File.ReadAllText(SourcePath);

        public void OnCompiled(object sender, ScriptEventArgs args) => Compiled?.Invoke(sender, args);
    }
}
