using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiceEngine.Entities;

namespace SauceEditor.Controls.Tools
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
