using SauceEditor.Models;
using SauceEditor.Models.Components;
using System;

namespace SauceEditor.Views.Scripts
{
    public class ScriptEventArgs : EventArgs
    {
        public Script Script { get; private set; }

        public ScriptEventArgs(Script script)
        {
            Script = script;
        }
    }
}
