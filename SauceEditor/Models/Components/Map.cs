using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;

namespace SauceEditor.Models.Components
{
    public class Map : Component
    {
        public SpiceEngine.Maps.Map GameMap { get; set; }

        /*public override void Save()
        {

        }*/
    }
}
