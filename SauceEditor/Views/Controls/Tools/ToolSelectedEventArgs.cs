using System;

namespace SauceEditor.Views.Controls.Tools
{
    public class ToolSelectedEventArgs : EventArgs
    {
        public SpiceEngine.Game.Tools NewTool { get; private set; }
        public SpiceEngine.Game.Tools OldTool { get; private set; }

        public ToolSelectedEventArgs(SpiceEngine.Game.Tools newTool, SpiceEngine.Game.Tools oldTool)
        {
            NewTool = newTool;
            OldTool = oldTool;
        }
    }
}
