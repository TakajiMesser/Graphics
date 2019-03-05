using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Scripting.Properties
{
    public class PropertyCollection
    {
        private Dictionary<string, Property> _propertiesByName = new Dictionary<string, Property>();

        public IEnumerable<Property> ConstantProperties => _propertiesByName.Values.Where(v => v.IsConstant);
        public IEnumerable<Property> VariableProperties => _propertiesByName.Values.Where(v => !v.IsConstant);

        public void AddProperties(IEnumerable<Property> properties)
        {
            foreach (var property in properties)
            {
                _propertiesByName.Add(property.Name, property);
            }
        }
    }
}
