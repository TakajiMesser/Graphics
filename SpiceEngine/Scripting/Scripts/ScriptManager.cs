using SpiceEngine.Entities;
using SpiceEngine.Entities.Cameras;
using SpiceEngine.Game;
using SpiceEngine.Inputs;
using SpiceEngine.Physics;
using SpiceEngine.Scripting.Nodes;
using SpiceEngine.Scripting.Properties;
using SpiceEngine.Scripting.StimResponse;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SpiceEngine.Scripting.Scripts
{
    /// <summary>
    /// A script is a set of sequential commands
    /// Each command can be something this object needs to communicate to another object, or performed by itself
    /// Each command either needs to be associated with 
    /// </summary>
    public class ScriptManager : IScriptCompiler
    {
        // Need to use new provider for Roslyn features
        private Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider _provider = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider();
        private CompilerParameters _compilerParameters;

        private Dictionary<string, Script> _scriptsByName = new Dictionary<string, Script>();

        public ScriptManager()
        {
            _compilerParameters = new CompilerParameters()
            {
                GenerateExecutable = false,
                GenerateInMemory = true
            };

            var executingAssembly = Assembly.GetExecutingAssembly();
            var assemblyPath = new Uri(executingAssembly.EscapedCodeBase).LocalPath;
            _compilerParameters.ReferencedAssemblies.Add(assemblyPath);

            //var assemblyNames = executingAssembly.GetReferencedAssemblies();
            //var openTKAssembly = assemblyNames.First(n => n.Name == "OpenTK");
            _compilerParameters.ReferencedAssemblies.Add("OpenTK.dll");
        }

        public void AddScript(Script script)
        {
            _scriptsByName.Add(script.Name, script);
        }

        public void CompileScripts()
        {
            var results = _provider.CompileAssemblyFromSource(_compilerParameters, _scriptsByName.Values.Select(v => v.GetContent()).ToArray());

            if (results.Errors.HasErrors)
            {
                // TODO - Handle errors by notifying user
                foreach (var error in results.Errors.Cast<CompilerError>())
                {
                    var script = _scriptsByName.Values.FirstOrDefault(v => Path.GetFileName(v.SourcePath) == error.FileName);
                    if (script != null)
                    {
                        script.Errors.Add(error);
                    }
                }
            }
            else
            {
                var assembly = results.CompiledAssembly;
                var types = assembly.GetExportedTypes();

                foreach (var type in types)
                {
                    if (_scriptsByName.ContainsKey(type.Name))
                    {
                        _scriptsByName[type.Name].ExportedType = type;
                    }
                }
            }
        }

        public void ClearScripts()
        {
            _scriptsByName.Clear();
        }
    }
}
