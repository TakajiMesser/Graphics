using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoEngine.Entities;

namespace SauceEditor.Controls.Tools
{
    public class ToolSelectedEventArgs : EventArgs
    {
        public TakoEngine.Game.Tools NewTool { get; private set; }
        public TakoEngine.Game.Tools OldTool { get; private set; }

        public ToolSelectedEventArgs(TakoEngine.Game.Tools newTool, TakoEngine.Game.Tools oldTool)
        {
            NewTool = newTool;
            OldTool = oldTool;
        }
    }
}
