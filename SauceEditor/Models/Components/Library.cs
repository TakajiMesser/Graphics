using System.Collections.Generic;

namespace SauceEditor.Models.Components
{
    public class Library<T> where T : Component
    {
        public List<T> Components { get; } = new List<T>();
    }
}
