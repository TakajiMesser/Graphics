using SpiceEngineCore.Scripting;
using System.Collections.Generic;
using System.Linq;

namespace UmamiScriptingCore.Behaviors.Properties
{
    public class PropertyCollection
    {
        private Dictionary<string, IProperty> _propertiesByName = new Dictionary<string, IProperty>();

        public IEnumerable<IProperty> ConstantProperties => _propertiesByName.Values.Where(v => v.IsConstant);
        public IEnumerable<IProperty> VariableProperties => _propertiesByName.Values.Where(v => !v.IsConstant);

        public void AddProperties(IEnumerable<IProperty> properties)
        {
            foreach (var property in properties)
            {
                _propertiesByName.Add(property.Name, property);
            }
        }
    }
}
