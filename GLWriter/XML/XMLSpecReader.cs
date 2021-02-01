using GLWriter.CSharp;
using GLWriter.XML.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace GLWriter.XML
{
    public class XMLSpecReader
    {
        private List<TypeSpec> _typeSpecs = new List<TypeSpec>();
        private List<EnumsSpec> _enumsSpecs = new List<EnumsSpec>();
        private List<CommandSpec> _commandSpecs = new List<CommandSpec>();
        private List<ExtensionSpec> _extensionSpecs = new List<ExtensionSpec>();

        public XMLSpecReader(string filePath) => FilePath = filePath;

        public string FilePath { get; }

        public void Parse()
        {
            using (var reader = XmlReader.Create(FilePath))
            {
                reader.MoveToContent();

                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "types":
                                ParseTypes(reader);
                                break;
                            case "enums":
                                ParseEnums(reader);
                                break;
                            case "commands":
                                ParseCommands(reader);
                                break;
                            case "extensions":
                                ParseExtensions(reader);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        private void ParseTypes(XmlReader reader)
        {
            TypeSpec typeSpec = null;
            var elementNames = new Stack<string>();

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        elementNames.Push(reader.Name);

                        if (XMLSpec.IsMatchingTag<TypeSpec>(reader))
                        {
                            typeSpec = XMLSpec.Parse<TypeSpec>(reader);

                            if (reader.IsEmptyElement)
                            {
                                _typeSpecs.Add(typeSpec);
                                typeSpec = null;
                            }
                        }
                        break;
                    case XmlNodeType.EndElement:
                        if (elementNames.Peek() == reader.Name)
                        {
                            elementNames.Pop();
                        }

                        if (XMLSpec.IsMatchingTag<TypeSpec>(reader))
                        {
                            _typeSpecs.Add(typeSpec);
                            typeSpec = null;
                        }
                        else if (reader.Name == "types")
                        {
                            return;
                        }
                        break;
                    case XmlNodeType.Text:
                        XMLSpec.ParseInnerElement(typeSpec, elementNames.Peek(), reader.Value);
                        break;
                }
            }
        }

        public static void ParseInnerElement<T>(T spec, XmlReader reader) where T : XMLSpec
        {
            if (spec != null)
            {
                if (!reader.IsEmptyElement)
                {
                    var name = reader.Name;
                    reader.Read();

                    if (reader.NodeType == XmlNodeType.Text)
                    {
                        spec.SetInnerElement(name, reader.Value);
                    }
                }
            }
        }

        private void ParseEnums(XmlReader reader)
        {
            var enumsSpec = new EnumsSpec();
            var isEndingElement = reader.IsEmptyElement;

            for (var i = 0; i < reader.AttributeCount; i++)
            {
                reader.MoveToAttribute(i);
                enumsSpec.SetAttribute(reader.Name, reader.Value);
            }

            if (isEndingElement)
            {
                _enumsSpecs.Add(enumsSpec);
                return;
            }

            EnumSpec enumSpec = null;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name == "enum")
                        {
                            enumSpec = new EnumSpec();

                            for (var i = 0; i < reader.AttributeCount; i++)
                            {
                                reader.MoveToAttribute(i);
                                enumSpec.SetAttribute(reader.Name, reader.Value);
                            }

                            if (!string.IsNullOrEmpty(enumsSpec.Group) && !enumSpec.GroupNames.Contains(enumsSpec.Group))
                            {
                                enumSpec.GroupNames.Add(enumsSpec.Group);
                            }

                            if (reader.IsEmptyElement)
                            {
                                enumsSpec.ChildSpecs.Add(enumSpec);
                                enumSpec = null;
                            }
                        }
                        break; 
                    case XmlNodeType.EndElement:
                        if (reader.Name == "enum")
                        {
                            enumsSpec.ChildSpecs.Add(enumSpec);
                            enumSpec = null;
                        }
                        if (reader.Name == "enums")
                        {
                            _enumsSpecs.Add(enumsSpec);
                            return;
                        }
                        break;
                }
            }
        }

        private void ParseCommands(XmlReader reader)
        {
            CommandSpec commandSpec = null;
            ProtoSpec protoSpec = null;
            ParamSpec paramSpec = null;
            var elementNames = new Stack<string>();

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        elementNames.Push(reader.Name);

                        if (XMLSpec.IsMatchingTag<CommandSpec>(reader))
                        {
                            commandSpec = XMLSpec.Parse<CommandSpec>(reader);
                        }
                        else if (XMLSpec.IsMatchingTag<ProtoSpec>(reader))
                        {
                            protoSpec = XMLSpec.Parse<ProtoSpec>(reader);

                            if (reader.IsEmptyElement)
                            {
                                commandSpec.Prototype = protoSpec;
                                protoSpec = null;
                            }
                        }
                        else if (XMLSpec.IsMatchingTag<ParamSpec>(reader))
                        {
                            paramSpec = XMLSpec.Parse<ParamSpec>(reader);

                            if (reader.IsEmptyElement)
                            {
                                commandSpec.Parameters.Add(paramSpec);
                                paramSpec = null;
                            }
                        }
                        break;
                    case XmlNodeType.EndElement:
                        if (elementNames.Peek() == reader.Name)
                        {
                            elementNames.Pop();
                        }

                        if (XMLSpec.IsMatchingTag<CommandSpec>(reader))
                        {
                            _commandSpecs.Add(commandSpec);
                            commandSpec = null;
                        }
                        else if (XMLSpec.IsMatchingTag<ProtoSpec>(reader))
                        {
                            commandSpec.Prototype = protoSpec;
                            protoSpec = null;
                        }
                        else if (XMLSpec.IsMatchingTag<ParamSpec>(reader))
                        {
                            commandSpec.Parameters.Add(paramSpec);
                            paramSpec = null;
                        }
                        else if (reader.Name == "commands")
                        {
                            return;
                        }
                        break;
                    case XmlNodeType.Text:
                    case XmlNodeType.Whitespace:
                        var elementName = elementNames.Count > 0 ? elementNames.Peek() : null;

                        XMLSpec.ParseInnerElement(commandSpec, elementName, reader.Value);
                        XMLSpec.ParseInnerElement(protoSpec, elementName, reader.Value);
                        XMLSpec.ParseInnerElement(paramSpec, elementName, reader.Value);
                        break;
                }
            }
        }

        private void ParseExtensions(XmlReader reader)
        {
            ExtensionSpec extensionSpec = null;
            RequireSpec requireSpec = null;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (XMLSpec.IsMatchingTag<ExtensionSpec>(reader))
                        {
                            extensionSpec = XMLSpec.Parse<ExtensionSpec>(reader);

                            if (reader.IsEmptyElement)
                            {
                                _extensionSpecs.Add(extensionSpec);
                                extensionSpec = null;
                            }
                        }
                        else if (XMLSpec.IsMatchingTag<RequireSpec>(reader))
                        {
                            requireSpec = XMLSpec.Parse<RequireSpec>(reader);

                            if (reader.IsEmptyElement)
                            {
                                extensionSpec.Requires.Add(requireSpec);
                                requireSpec = null;
                            }
                        }
                        else if (XMLSpec.IsMatchingTag<Extensions.EnumSpec>(reader))
                        {
                            var enumSpec = XMLSpec.Parse<Extensions.EnumSpec>(reader);
                            requireSpec.Enums.Add(enumSpec);
                        }
                        else if (XMLSpec.IsMatchingTag<Extensions.CommandSpec>(reader))
                        {
                            var commandSpec = XMLSpec.Parse<Extensions.CommandSpec>(reader);
                            requireSpec.Commands.Add(commandSpec);
                        }
                        break;
                    case XmlNodeType.EndElement:
                        if (XMLSpec.IsMatchingTag<ExtensionSpec>(reader))
                        {
                            _extensionSpecs.Add(extensionSpec);
                            extensionSpec = null;
                        }
                        else if (XMLSpec.IsMatchingTag<RequireSpec>(reader))
                        {
                            extensionSpec.Requires.Add(requireSpec);
                            requireSpec = null;
                        }
                        else if (reader.Name == "extensions")
                        {
                            return;
                        }
                        break;
                }
            }
        }

        public IEnumerable<EnumGroup> ProcessEnums()
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

                        var enumValue = new EnumValue(enumSpec.Name, enumSpec.Value);
                        groups[groupName].Values.Add(enumValue);
                    }
                }
            }

            return groups.Values;
        }

        public IEnumerable<Function> ProcessFunctions()
        {
            foreach (var commandSpec in _commandSpecs)
            {
                if (IsExtensionCommand(commandSpec.Prototype.Name))
                {
                    continue;
                }

                var function = new Function()
                {
                    Name = commandSpec.Prototype.Name,
                    ReturnType = GetDataType(commandSpec.Prototype.Type, commandSpec.Prototype.Content, commandSpec.Prototype.Group)
                };

                if (function.ReturnType == DataTypes.ENUM)
                {
                    function.EnumName = commandSpec.Prototype.Group;
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
                        parameter.EnumName = paramSpec.Group;
                    }

                    function.Parameters.Add(parameter);
                }

                yield return function;
            }
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
            if (type == "struct _cl_context"
                        || type == "struct _cl_event"
                        || type == "GLhandleARB"
                        || type == "GLvdpauSurfaceNV")
            {
                return DataTypes.None;
            }

            var glType = GLTypeExtensions.ParseGLType(type);
            var dataType = glType.ToDataType();
            var nPointers = content.Count(c => c == '*');

            // Handle void
            if (dataType == DataTypes.None && ContentContains(content, "void"))
            {
                dataType = DataTypes.VOID;
            }

            // Handle out
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
