using SpiceEngineCore.Scripting.Scripts;
using System;

namespace SauceEditorCore.Models.Components
{
    public class ScriptComponent : Component
    {
        public ScriptComponent(string filePath) : base(filePath) { }

        public Script Script { get; set; }

        public override void Save() => throw new NotImplementedException();
        public override void Load() => throw new NotImplementedException();

        public static bool IsValidExtension(string extension) => extension == "cs";
    }
}
