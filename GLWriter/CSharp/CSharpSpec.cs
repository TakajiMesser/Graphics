using System.Collections.Generic;

namespace GLWriter.CSharp
{
    public class CSharpSpec
    {
        private List<EnumGroup> _enumGroups = new List<EnumGroup>();
        private List<Function> _functions = new List<Function>();
        private List<Delegate> _delegates = new List<Delegate>();

        public IEnumerable<EnumGroup> Enums => _enumGroups;
        public IEnumerable<Function> Functions => _functions;
        public int DelegateCount => _delegates.Count;

        public Delegate DelegateAt(int index) => _delegates[index];

        public void AddEnum(EnumGroup enumGroup) => _enumGroups.Add(enumGroup);
        public void AddFunction(Function function) => _functions.Add(function);

        public void AddEnums(IEnumerable<EnumGroup> enumGroups) => _enumGroups.AddRange(enumGroups);
        public void AddFunctions(IEnumerable<Function> functions) => _functions.AddRange(functions);

        public void Process()
        {
            var delegateByName = new Dictionary<string, Delegate>();

            foreach (var function in _functions)
            {
                var delegateDefinition = new Delegate(function);
                if (!delegateByName.ContainsKey(delegateDefinition.Name))
                {
                    _delegates.Add(delegateDefinition);
                    delegateByName.Add(delegateDefinition.Name, delegateDefinition);
                }

                function.Delegate = delegateByName[delegateDefinition.Name];
            }

            _enumGroups.Sort((a, b) => a.Name.CompareTo(b.Name));
            _functions.Sort((a, b) => a.Name.CompareTo(b.Name));
            _delegates.Sort((a, b) => a.Name.CompareTo(b.Name));
        }
    }
}
