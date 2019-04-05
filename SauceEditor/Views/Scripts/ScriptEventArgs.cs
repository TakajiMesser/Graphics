using SauceEditor.Models;
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
