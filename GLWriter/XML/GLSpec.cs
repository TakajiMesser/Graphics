using GLWriter.CSharp;
using GLWriter.XML.Enums;
using GLWriter.XML.Extensions;
using GLWriter.XML.Features;
using GLWriter.XML.Types;
using System.Collections.Generic;
using System.Linq;
using CommandSpec = GLWriter.XML.Commands.CommandSpec;
using TypeSpec = GLWriter.XML.Types.TypeSpec;

namespace GLWriter.XML
{
    public class GLSpec
    {
        private List<TypeSpec> _typeSpecs = new List<TypeSpec>();
        private List<EnumsSpec> _enumsSpecs = new List<EnumsSpec>();
        private List<CommandSpec> _commandSpecs = new List<CommandSpec>();
        private List<FeatureSpec> _featureSpecs = new List<FeatureSpec>();
        private List<ExtensionSpec> _extensionSpecs = new List<ExtensionSpec>();

        public void AddType(TypeSpec spec) => _typeSpecs.Add(spec);
        public void AddEnum(EnumsSpec spec) => _enumsSpecs.Add(spec);
        public void AddCommand(CommandSpec spec) => _commandSpecs.Add(spec);
        public void AddFeature(FeatureSpec spec) => _featureSpecs.Add(spec);
        public void AddExtension(ExtensionSpec spec) => _extensionSpecs.Add(spec);

        public CSharpSpec GenerateCSharpSpec(Version version)
        {
            var spec = new CSharpSpec();
            spec.AddEnums(ProcessEnums(version));
            spec.AddFunctions(ProcessFunctions(version));
            spec.Process();

            return spec;
        }

        public IEnumerable<EnumGroup> ProcessEnums(Version version)
        {
            var groups = new Dictionary<string, EnumGroup>();

            foreach (var enumsSpec in _enumsSpecs)
            {
                foreach (var enumSpec in enumsSpec.ChildSpecs)
                {
                    foreach (var groupName in enumSpec.GroupNames)
                    {
                        if (!groups.ContainsKey(groupName))
                        {
                            groups.Add(groupName, new EnumGroup(groupName));
                        }

                        if (IsSupportedEnum(enumSpec.Name, version))
                        {
                            var enumValue = new EnumValue(enumSpec.Name, enumSpec.Value);
                            groups[groupName].Values.Add(enumValue);
                        }
                    }
                }
            }

            return groups.Values.Where(v => v.Values.Count > 0);
        }

        public IEnumerable<Function> ProcessFunctions(Version version)
        {
            foreach (var commandSpec in _commandSpecs)
            {
                if (IsSupportedCommand(commandSpec.Prototype.Name, version))
                {
                    var function = new Function()
                    {
                        Name = commandSpec.Prototype.Name,
                        ReturnType = GetDataType(commandSpec.Prototype.Type, commandSpec.Prototype.Content, commandSpec.Prototype.Group)
                    };

                    if (function.ReturnType == DataTypes.ENUM)
                    {
                        function.Group = commandSpec.Prototype.Group;
                    }

                    foreach (var paramSpec in commandSpec.Parameters)
                    {
                        var parameter = new Parameter()
                        {
                            Name = paramSpec.Name,
                            DataType = GetDataType(paramSpec.Type, paramSpec.Content, paramSpec.Group)
                        };

                        if (parameter.DataType == DataTypes.ENUM || parameter.DataType == DataTypes.ENUMPTR)
                        {
                            parameter.Group = paramSpec.Group;
                        }

                        function.Parameters.Add(parameter);
                    }

                    yield return function;
                }
            }
        }

        private bool IsSupportedType(string name, Version version)
        {
            foreach (var feature in _featureSpecs)
            {
                if (feature.API == version.API)
                {
                    foreach (var require in feature.Requires)
                    {
                        foreach (var type in require.Types)
                        {
                            if (name == type.Name)
                            {
                                if (version.CompareTo(feature.Version) >= 0)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool IsSupportedEnum(string name, Version version)
        {
            foreach (var feature in _featureSpecs)
            {
                if (feature.API == version.API)
                {
                    foreach (var require in feature.Requires)
                    {
                        foreach (var enumValue in require.Enums)
                        {
                            if (name == enumValue.Name)
                            {
                                if (version.CompareTo(feature.Version) >= 0)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool IsSupportedCommand(string name, Version version)
        {
            foreach (var feature in _featureSpecs)
            {
                if (feature.API == version.API)
                {
                    foreach (var require in feature.Requires)
                    {
                        foreach (var command in require.Commands)
                        {
                            if (name == command.Name)
                            {
                                if (version.CompareTo(feature.Version) >= 0)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool IsExtensionCommand(string commandName)
        {
            foreach (var extension in _extensionSpecs)
            {
                foreach (var require in extension.Requires)
                {
                    foreach (var command in require.Commands)
                    {
                        if (command.Name == commandName)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private DataTypes GetDataType(string type, string content, string group)
        {
            var glType = GLTypeExtensions.ParseGLType(type);
            var dataType = glType.ToDataType();
            var nPointers = content.Count(c => c == '*');

            if (dataType == DataTypes.None && ContentContains(content, "void"))
            {
                dataType = DataTypes.VOID;
            }

            if (ContentContains(content, "out"))
            {
                return dataType.ToOutDataType();
            }

            if (nPointers == 0)
            {
                return dataType;
            }
            else if (nPointers == 1)
            {
                return dataType.ToPtrType();
            }
            else if (nPointers == 2)
            {
                return dataType.ToPtrPtrType();
            }
            else
            {
                throw new System.Exception("");
            }
        }

        private bool ContentContains(string content, string match)
        {
            var contents = content.Split(" ");

            foreach (var word in contents)
            {
                if (word == match)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
