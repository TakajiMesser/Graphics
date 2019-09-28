using SpiceEngine.Scripting.Scripts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SauceEditorCore.Models.Components
{
    public class ScriptComponent : Component
    {
        public ScriptComponent(string filePath) : base(filePath) { }

        public Script Script { get; set; }

        public override void Save() => throw new NotImplementedException();
        public override void Load() => throw new NotImplementedException();
    }
}
