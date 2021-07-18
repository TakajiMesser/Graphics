using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using UmamiScriptingCore.Scripts;

namespace SpiceEngine.Scripting
{
    public class ScriptManager : IScriptCompiler
    {
        // Need to use new provider for Roslyn features
        //private Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider _provider = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider();
        private CSharpCodeProvider _provider = new CSharpCodeProvider();
        private CompilerParameters _compilerParameters;
        private CSharpCompilation _compilation;

        private Dictionary<string, List<IScript>> _scriptsByName = new Dictionary<string, List<IScript>>();

        private object _compilerLock = new object();

        public event EventHandler Compiled;

        public ScriptManager() => InitializeNewCompiler();

        private void InitializeNewCompiler()
        {
            var assemblyName = Path.GetRandomFileName();

            _compilation = CSharpCompilation.Create(assemblyName)
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            /*var refPaths = new[]
            {
                typeof(object).GetTypeInfo().Assembly.Location,
                typeof(Console).GetTypeInfo().Assembly.Location,
                Path.Combine(Path.GetDirectoryName(typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly.Location), "System.Runtime.dll")
            };*/

            //_compilation = _compilation.AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));

            /*var trustedAssembliesPaths = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")).Split(Path.PathSeparator);

            if (!referenceAssemblies.Any(a => a.Contains("mscorlib")))
            {
                referenceAssemblies.Add("mscorlib.dll");
            }

            var references = trustedAssembliesPaths.Where(p => referenceAssemblies.Contains(Path.GetFileName(p)))
                                                        .Select(p => MetadataReference.CreateFromFile(p))
                                                        .ToList();*/

            //_compilation.AddReferences(refPaths.Select(r => MetadataReference.CreateFromFile(r)));

            var assembly = Assembly.GetExecutingAssembly();
            var assemblyPath = new Uri(assembly.Location).LocalPath;
            _compilation = _compilation.AddReferences(MetadataReference.CreateFromFile(assemblyPath));

            var domainDLLs = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetModules())
                .Select(m => m.FullyQualifiedName)
                .Where(n => n.EndsWith(".dll"));

            foreach (var dll in domainDLLs)
            {
                _compilation = _compilation.AddReferences(MetadataReference.CreateFromFile(dll));
            }

            /*_compilation = _compilation.AddReferences(MetadataReference.CreateFromFile("SpiceEngine.Core.dll"));
            _compilation = _compilation.AddReferences(MetadataReference.CreateFromFile("SpiceEngine.GLFWBindings.dll"));
            _compilation = _compilation.AddReferences(MetadataReference.CreateFromFile("CitrusAnimation.dll"));
            _compilation = _compilation.AddReferences(MetadataReference.CreateFromFile("SavoryPhysics.dll"));
            _compilation = _compilation.AddReferences(MetadataReference.CreateFromFile("SmokyAudio.dll"));
            _compilation = _compilation.AddReferences(MetadataReference.CreateFromFile("StarchUI.dll"));
            _compilation = _compilation.AddReferences(MetadataReference.CreateFromFile("SweetGraphics.dll"));
            _compilation = _compilation.AddReferences(MetadataReference.CreateFromFile("TangyHID.dll"));
            _compilation = _compilation.AddReferences(MetadataReference.CreateFromFile("UmamiScripting.dll"));*/
        }

        private void InitializeOldCompiler()
        {
            _compilerParameters = new CompilerParameters()
            {
                GenerateExecutable = false,
                GenerateInMemory = true
            };

            var assembly = Assembly.GetExecutingAssembly();
            var assemblyPath = new Uri(assembly.Location).LocalPath;
            _compilerParameters.ReferencedAssemblies.Add(assemblyPath);

            var domainDLLs = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetModules())
                .Select(m => m.Name)
                .Where(n => n.EndsWith(".dll"));

            foreach (var dll in domainDLLs)
            {
                _compilerParameters.ReferencedAssemblies.Add(dll);
            }

            //var assemblyNames = executingAssembly.GetReferencedAssemblies();
            //var openTKAssembly = assemblyNames.First(n => n.Name == "OpenTK");
            //_compilerParameters.ReferencedAssemblies.Add("");
            //_compilerParameters.ReferencedAssemblies.Add("System.Core.dll");
            //_compilerParameters.ReferencedAssemblies.Add("netstandard.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.Private.CoreLib.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.Runtime.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.Collections.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.Linq.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.Resources.Extensions.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.Memory.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.Threading.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.Runtime.Serialization.Formatters.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.Collections.Concurrent.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.Runtime.Serialization.Primitives.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.Runtime.Numerics.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.Private.Uri.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.Diagnostics.TraceSource.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.Linq.Expressions.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.ComponentModel.TypeConverter.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.ObjectModel.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.Collections.Specialized.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.Data.Common.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.Xml.ReaderWriter.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.Private.Xml.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.ComponentModel.Primitives.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.ComponentModel.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.Reflection.Emit.ILGeneration.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.Reflection.Primitives.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.Reflection.Emit.Lightweight.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.IO.FileSystem.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.Reflection.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.Runtime.InteropServices.RuntimeInformation.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.Runtime.Extensions.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.AppContext.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.Runtime.InteropServices.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.Reflection.Extensions.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.Text.Encoding.dll");
            _compilerParameters.ReferencedAssemblies.Add("System.CodeDom.dll");

            _compilerParameters.ReferencedAssemblies.Add("AssimpNet.dll");
            _compilerParameters.ReferencedAssemblies.Add("netstandard.dll");
            _compilerParameters.ReferencedAssemblies.Add("SpiceEngine.Full.dll");

            _compilerParameters.ReferencedAssemblies.Add("OpenTK.dll");
            _compilerParameters.ReferencedAssemblies.Add("Newtonsoft.Json.dll");
            _compilerParameters.ReferencedAssemblies.Add("SpiceEngine.Core.dll");
            _compilerParameters.ReferencedAssemblies.Add("SpiceEngine.GLFWBindings.dll");
            _compilerParameters.ReferencedAssemblies.Add("CitrusAnimation.dll");
            _compilerParameters.ReferencedAssemblies.Add("SavoryPhysics.dll");
            _compilerParameters.ReferencedAssemblies.Add("SmokyAudio.dll");
            _compilerParameters.ReferencedAssemblies.Add("StarchUI.dll");
            _compilerParameters.ReferencedAssemblies.Add("SweetGraphics.dll");
            _compilerParameters.ReferencedAssemblies.Add("TangyHID.dll");
            _compilerParameters.ReferencedAssemblies.Add("UmamiScripting.dll");
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
            CompilerResults results;

            lock (_compilerLock)
            {
                var syntaxTrees = _scriptsByName.Values.Select(v => v.First().Contents).Select(c => CSharpSyntaxTree.ParseText(c));
                _compilation = _compilation.AddSyntaxTrees(syntaxTrees);

                using (var stream = new MemoryStream())
                {
                    var result = _compilation.Emit(stream);

                    if (!result.Success)
                    {
                        foreach (var diagnostic in result.Diagnostics.Where(d => d.IsWarningAsError || d.Severity == DiagnosticSeverity.Error))
                        {
                            var script = _scriptsByName.Values.FirstOrDefault(v => v.First().Name == diagnostic.Location.ToString());
                            if (script != null)
                            {
                                script.First().Errors.Add(diagnostic.GetMessage());
                            }
                        }

                        throw new Exception("");
                    }
                    else
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        var assembly = AssemblyLoadContext.Default.LoadFromStream(stream);
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

                        /*var type = assembly.GetType("RoslynCompileSample.Writer");
                        var instance = assembly.CreateInstance("RoslynCompileSample.Writer");
                        var meth = type.GetMember("Write").First() as MethodInfo;
                        meth.Invoke(instance, new[] { "joel" });*/
                    }
                }

                /*results = _provider.CompileAssemblyFromSource(_compilerParameters, _scriptsByName.Values.Select(v => v.First().Contents).ToArray());

                if (results.Errors.HasErrors)
                {
                    // TODO - Handle errors by notifying user
                    foreach (var error in results.Errors.Cast<CompilerError>())
                    {
                        var script = _scriptsByName.Values.FirstOrDefault(v => v.First().Name == error.FileName);
                        if (script != null)
                        {
                            script.First().Errors.Add(error.ToString());
                        }
                    }

                    throw new Exception();
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
                }*/
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
