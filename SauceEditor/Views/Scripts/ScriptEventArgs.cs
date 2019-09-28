using SauceEditor.Models;
using SauceEditor.Models.Components;
using SauceEditorCore.Models.Components;
using System;

namespace SauceEditor.Views.Scripts
{
    public class ScriptEventArgs : EventArgs
    {
        public ScriptComponent Script { get; private set; }

        public ScriptEventArgs(ScriptComponent script)
        {
            Script = script;
        }
    }
}
