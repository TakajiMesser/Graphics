using System;

namespace SpiceEngineCore.Scripting
{
    public class ScriptEventArgs : EventArgs
    {
        public IScript Script { get; private set; }

        public ScriptEventArgs(IScript script) => Script = script;
    }
}
