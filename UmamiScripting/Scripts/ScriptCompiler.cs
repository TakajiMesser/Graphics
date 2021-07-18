namespace UmamiScriptingCore.Scripts
{
    /*public class ScriptManager : IScriptCompiler
    {
        // Need to use new provider for Roslyn features
        private CSharpCodeProvider _provider = new CSharpCodeProvider();
        private CompilerParameters _compilerParameters;

        private Dictionary<string, List<IScript>> _scriptsByName = new Dictionary<string, List<IScript>>();

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

            var domainDLLs = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetModules())
                .Select(m => m.Name)
                .Where(n => n.EndsWith(".dll"));

            foreach (var dll in domainDLLs)
            {
                _compilerParameters.ReferencedAssemblies.Add(dll);
            }
        }

        public void AddScript(IScript script)
        {
            lock (_compilerLock)
            {
                throw new NotImplementedException();
                //_scriptsByName.Add(script.Name, script);
            }
        }

        public void AddScripts(IEnumerable<IScript> scripts)
        {
            lock (_compilerLock)
            {
                foreach (var script in scripts)
                {
                    // TODO - This won't work, as the duplicate scripts won't fire the necessary compilation event
                    if (!_scriptsByName.ContainsKey(script.Name))
                    {
                        _scriptsByName.Add(script.Name, new List<IScript>());
                    }

                    _scriptsByName[script.Name].Add(script);
                }
            }
        }

        public void CompileScripts()
        {
            var provider = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider();
            var compiler = provider.CreateCompiler();

            CSharpCodeProvider

            Microsoft.CodeAnalysis.CSharp.Scripting.CSharpScript.EvaluateAsync

            CompilerResults results;

            lock (_compilerLock)
            {
                _provider.CreateCompiler();

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
            }*
        }

        public void ClearScripts()
        {
            lock (_compilerLock)
            {
                _scriptsByName.Clear();
            }
        }
    }*/
}
