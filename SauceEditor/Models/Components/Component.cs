using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;

namespace SauceEditor.Models.Components
{
    public abstract class Component
    {
        public string Name { get; set; }
        public string Path { get; set; }

        //public abstract void Save();
    }
}
