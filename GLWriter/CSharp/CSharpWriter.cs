using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GLWriter.CSharp
{
    public class CSharpWriter
    {
        private List<EnumGroup> _enumGroups = new List<EnumGroup>();
        private List<Function> _functions = new List<Function>();
        private List<Delegate> _delegates = new List<Delegate>();

        public void AddEnums(IEnumerable<EnumGroup> enumGroups) => _enumGroups.AddRange(enumGroups);
        public void AddFunctions(IEnumerable<Function> functions) => _functions.AddRange(functions);

        public void Process()
        {
            var functions = new List<Function>();
            var delegates = new List<Delegate>();
            var delegateByName = new Dictionary<string, Delegate>();

            foreach (var function in _functions.OrderBy(f => f.Name))
            {
                if (!IsSupported(function)) continue;
                functions.Add(function);

                var delegateDefinition = new Delegate(function);
                if (!delegateByName.ContainsKey(delegateDefinition.Name))
                {
                    _delegates.Add(delegateDefinition);
                    delegateByName.Add(delegateDefinition.Name, delegateDefinition);
                }

                function.Delegate = delegateByName[delegateDefinition.Name];
            }

            foreach (var delegateDefinition in _delegates.OrderBy(d => d.Name))
            {
                delegates.Add(delegateDefinition);
            }

            _functions = functions;
            _delegates = delegates;
        }

        private bool IsSupported(Function function)
        {
            if (function.ReturnType == DataTypes.None)
            {
                return false;
            }

            foreach (var parameter in function.Parameters)
            {
                if (parameter.DataType == DataTypes.None)
                {
                    return false;
                }
            }

            return true;
        }

        public void WriteEnumFiles(string directoryPath)
        {
            foreach (var enumGroup in _enumGroups)
            {
                var filePath = Path.Join(directoryPath, enumGroup.Name + ".cs");
                File.WriteAllLines(filePath, enumGroup.ToLines());
            }
        }

        public void WriteFunctionFile(string filePath)
        {
            var lines = new List<string>();
            lines.Add("using System;");
            lines.Add("using System.Runtime.InteropServices;");
            lines.Add("");
            lines.Add("namespace SpiceEngineCore.GLFW");
            lines.Add("{");
            lines.Add("    public static unsafe class GLCommands");
            lines.Add("    {");

            foreach (var function in _functions)
            {
                lines.Add("        " + function.ToFieldLine());
            }

            lines.Add("        public static void LoadFunctions()");
            lines.Add("        {");

            foreach (var function in _functions)
            {
                lines.Add("            " + function.ToLoadLine());
            }

            lines.Add("        }");
            lines.Add("");

            foreach (var function in _functions)
            {
                lines.Add("        " + function.ToDefinitionLine());
            }

            lines.Add("        private static T GetFunctionDelegate<T>(string name) where T : Delegate");
            lines.Add("        {");
            lines.Add("            var address = GLFW.GetProcAddress(name);");
            lines.Add("            return Marshal.GetDelegateForFunctionPointer<T>(address);");
            lines.Add("        }");
            lines.Add("");

            for (var i = 0; i < _delegates.Count; i++)
            {
                var delegateDefinition = _delegates[i];

                lines.Add("        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]");
                lines.Add("        " + delegateDefinition.ToDefinitionLine());

                if (i < _delegates.Count - 1)
                {
                    lines.Add("");
                }
            }

            lines.Add("    }");
            lines.Add("}");

            File.WriteAllLines(filePath, lines);
        }
    }
}
