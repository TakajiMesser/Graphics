using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceEditor.Controls
{
    public class MapEventArgs : EventArgs
    {
        public string MapFile { get; private set; }

        public MapEventArgs(string mapFile)
        {
            MapFile = mapFile;
        }
    }
}
