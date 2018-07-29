using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SpiceEngine.Game;

namespace SauceEditor.Structure
{
    public class SettingsEventArgs : EventArgs
    {
        public EditorSettings Settings { get; private set; }

        public SettingsEventArgs(EditorSettings settings)
        {
            Settings = settings;
        }
    }
}
