using SpiceEngineCore.Scripting.Scripts;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SpiceEngine.Scripting.Scripts
{
    public class ScriptManager : IScriptCompiler
    {
        // Need to use new provider for Roslyn features
        private Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider _provider = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider();
        private CompilerParameters _compilerParameters;

        private Dictionary<string, List<Script>> _scriptsByName = new Dictionary<string, List<Script>>();

        private object _compilerLock = new object();

        public event EventHandler Compiled;

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
            _compilerParameters.ReferencedAssemblies.Add("netstandard.dll");
            _compilerParameters.ReferencedAssemblies.Add("SpiceEngineCore.dll");
            _compilerParameters.ReferencedAssemblies.Add("OpenTK.dll");
            _compilerParameters.ReferencedAssemblies.Add("Newtonsoft.Json.dll");
        }

        public void AddScript(Script script)
        {
            lock (_compilerLock)
            {
                throw new NotImplementedException();
                //_scriptsByName.Add(script.Name, script);
            }
        }

        public void AddScripts(IEnumerable<Script> scripts)
        {
            lock (_compilerLock)
            {
                foreach (var script in scripts)
                {
                    // TODO - This won't work, as the duplicate scripts won't fire the necessary compilation event
                    if (!_scriptsByName.ContainsKey(script.Name))
                    {
                        _scriptsByName.Add(script.Name, new List<Script>());
                    }

                    _scriptsByName[script.Name].Add(script);
                }
            }
        }

        public void CompileScripts()
        {
            CompilerResults results;

            lock (_compilerLock)
            {
                results = _provider.CompileAssemblyFromSource(_compilerParameters, _scriptsByName.Values.Select(v => v.First().GetContent()).ToArray());

                if (results.Errors.HasErrors)
                {
                    // TODO - Handle errors by notifying user
                    foreach (var error in results.Errors.Cast<CompilerError>())
                    {
                        var script = _scriptsByName.Values.FirstOrDefault(v => Path.GetFileName(v.First().SourcePath) == error.FileName);
                        if (script != null)
                        {
                            script.First().Errors.Add(error.ToString());
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
                            foreach (var script in _scriptsByName[type.Name])
                            {
                                script.ExportedType = type;
                                script.OnCompiled(this, new ScriptEventArgs(script));
                            }
                            //_scriptsByName[type.Name].ExportedType = type;
                            //_scriptsByName[type.Name].OnCompiled(this, new ScriptEventArgs(_scriptsByName[type.Name]));
                        }
                    }
                }
            }

            /*if (results.Errors.HasErrors)
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
            }*/
        }

        public void ClearScripts()
        {
            lock (_compilerLock)
            {
                _scriptsByName.Clear();
            }
        }
    }
}
