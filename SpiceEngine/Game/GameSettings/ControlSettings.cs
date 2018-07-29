using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiceEngine.Inputs;

namespace SpiceEngine.Game.GameSettings
{
    public class ControlSettings
    {
        public InputMapping InputMapping { get; set; }

        public int MouseXSensitivity { get; set; }
        public int MouseYSensitivity { get; set; }
    }
}
