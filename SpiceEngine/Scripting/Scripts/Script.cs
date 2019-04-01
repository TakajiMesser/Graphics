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
        public string Name { get; }
        public string SourcePath { get; }

        [IgnoreDataMember]
        public Type ExportedType { get; private set; }

        [IgnoreDataMember]
        public List<CompileError> Errors { get; private set; } = new List<CompileError>();

        [IgnoreDataMember]
        public bool HasErrors => Errors.Count > 0;

        [IgnoreDataMember]
        public bool IsCompiled => ExportedType != null || HasErrors;

        public Script(string name, string sourcePath)
        {
            Name = name;
            SourcePath = sourcePath;
        }
    }
}
