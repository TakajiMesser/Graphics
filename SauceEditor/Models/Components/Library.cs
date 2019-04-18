using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;

namespace SauceEditor.Models.Components
{
    public class Library<T> where T : Component
    {
        public List<T> Components { get; } = new List<T>();
    }
}
