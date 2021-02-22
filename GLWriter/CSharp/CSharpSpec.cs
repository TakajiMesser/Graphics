using System.Collections.Generic;

namespace GLWriter.CSharp
{
    public class CSharpSpec
    {
        private List<EnumGroup> _enumGroups = new List<EnumGroup>();
        private List<Struct> _structs = new List<Struct>();
        private List<Function> _functions = new List<Function>();
        private List<Overload> _overloads = new List<Overload>();
        private List<Delegate> _delegates = new List<Delegate>();

        public IEnumerable<EnumGroup> Enums => _enumGroups;
        public IEnumerable<Struct> Structs => _structs;
        public IEnumerable<Function> Functions => _functions;

        public int OverloadCount => _overloads.Count;
        public int DelegateCount => _delegates.Count;
        public Overload OverloadAt(int index) => _overloads[index];
        public Delegate DelegateAt(int index) => _delegates[index];

        public void AddEnum(EnumGroup enumGroup) => _enumGroups.Add(enumGroup);
        public void AddStruct(Struct structSpec) => _structs.Add(structSpec);
        public void AddFunction(Function function) => _functions.Add(function);

        public void AddStructs(IEnumerable<Struct> structs) => _structs.AddRange(structs);
        public void AddEnums(IEnumerable<EnumGroup> enumGroups) => _enumGroups.AddRange(enumGroups);
        public void AddFunctions(IEnumerable<Function> functions) => _functions.AddRange(functions);

        public void Process()
        {
            var enumNames = new HashSet<string>();
            var functionByName = new Dictionary<string, Function>();
            var delegateByName = new Dictionary<string, Delegate>();

            foreach (var enumGroup in _enumGroups)
            {
                enumNames.Add(enumGroup.Name);
            }

            foreach (var function in _functions)
            {
                functionByName.Add(function.Name, function);

                for (var i = 0; i < function.Parameters.Count; i++)
                {
                    var parameterType = function.Parameters[i].Type;

                    if (parameterType.DataType == DataTypes.Int && !string.IsNullOrEmpty(parameterType.Group) && enumNames.Contains(parameterType.Group))
                    {
                        function.Parameters[i] = function.Parameters[i].Converted(DataTypes.Enum);
                    }
                    else if (parameterType.DataType == DataTypes.Enum && (string.IsNullOrEmpty(parameterType.Group) || !enumNames.Contains(parameterType.Group)))
                    {
                        function.Parameters[i] = function.Parameters[i].Converted(DataTypes.Int);
                    }
                }

                var delegateDefinition = new Delegate(function);
                if (!delegateByName.ContainsKey(delegateDefinition.Name))
                {
                    _delegates.Add(delegateDefinition);
                    delegateByName.Add(delegateDefinition.Name, delegateDefinition);
                }

                function.Delegate = delegateByName[delegateDefinition.Name];

                foreach (var overload in Overload.ForFunction(function))
                {
                    _overloads.Add(overload);
                }
            }

            for (var i = _overloads.Count - 1; i >= 0; i--)
            {
                var overload = _overloads[i];

                if (functionByName.ContainsKey(overload.Name))
                {
                    var function = functionByName[overload.Name];

                    if (overload.IsDuplicateOf(function))
                    {
                        _overloads.RemoveAt(i);
                    }
                }
            }

            _enumGroups.Sort((a, b) => a.Name.CompareTo(b.Name));
            _structs.Sort((a, b) => a.Name.CompareTo(b.Name));
            _functions.Sort((a, b) => a.Name.CompareTo(b.Name));
            _overloads.Sort((a, b) => a.Name.CompareTo(b.Name));
            _delegates.Sort((a, b) => a.Name.CompareTo(b.Name));
        }
    }
}
