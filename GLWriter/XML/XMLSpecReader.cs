using GLWriter.XML.Commands;
using GLWriter.XML.Enums;
using GLWriter.XML.Extensions;
using GLWriter.XML.Features;
using System.Collections.Generic;
using System.Xml;
using CommandSpec = GLWriter.XML.Commands.CommandSpec;
using TypeSpec = GLWriter.XML.Types.TypeSpec;

namespace GLWriter.XML
{
    public class XMLSpecReader
    {
        public XMLSpecReader(string filePath) => FilePath = filePath;

        public string FilePath { get; }
        public GLSpec Spec { get; } = new GLSpec();

        public void Parse()
        {
            using var reader = XmlReader.Create(FilePath);
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
                        case "feature":
                            ParseFeature(reader);
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
                            var isEmptyElement = reader.IsEmptyElement;
                            typeSpec = XMLSpec.Parse<TypeSpec>(reader);

                            if (isEmptyElement)
                            {
                                Spec.AddType(typeSpec);
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
                            Spec.AddType(typeSpec);
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
                Spec.AddEnum(enumsSpec);
                return;
            }

            Enums.EnumSpec enumSpec = null;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (XMLSpec.IsMatchingTag<Enums.EnumSpec>(reader))
                        {
                            var isEmptyElement = reader.IsEmptyElement;
                            enumSpec = XMLSpec.Parse<Enums.EnumSpec>(reader);

                            if (!string.IsNullOrEmpty(enumsSpec.Group) && !enumSpec.GroupNames.Contains(enumsSpec.Group))
                            {
                                enumSpec.GroupNames.Add(enumsSpec.Group);
                            }

                            if (isEmptyElement)
                            {
                                enumsSpec.ChildSpecs.Add(enumSpec);
                                enumSpec = null;
                            }
                        }
                        break; 
                    case XmlNodeType.EndElement:
                        if (XMLSpec.IsMatchingTag<Enums.EnumSpec>(reader))
                        {
                            enumsSpec.ChildSpecs.Add(enumSpec);
                            enumSpec = null;
                        }
                        if (reader.Name == "enums")
                        {
                            Spec.AddEnum(enumsSpec);
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
                            var isEmptyElement = reader.IsEmptyElement;
                            protoSpec = XMLSpec.Parse<ProtoSpec>(reader);

                            if (isEmptyElement)
                            {
                                commandSpec.Prototype = protoSpec;
                                protoSpec = null;
                            }
                        }
                        else if (XMLSpec.IsMatchingTag<ParamSpec>(reader))
                        {
                            var isEmptyElement = reader.IsEmptyElement;
                            paramSpec = XMLSpec.Parse<ParamSpec>(reader);

                            if (isEmptyElement)
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
                            Spec.AddCommand(commandSpec);
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

        private void ParseFeature(XmlReader reader)
        {
            var featureSpec = XMLSpec.Parse<FeatureSpec>(reader);

            if (reader.IsEmptyElement)
            {
                Spec.AddFeature(featureSpec);
                return;
            }

            Features.RequireSpec requireSpec = null;
            RemoveSpec removeSpec = null;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (XMLSpec.IsMatchingTag<Features.RequireSpec>(reader))
                        {
                            var isEmptyElement = reader.IsEmptyElement;
                            requireSpec = XMLSpec.Parse<Features.RequireSpec>(reader);

                            if (isEmptyElement)
                            {
                                featureSpec.Requires.Add(requireSpec);
                                requireSpec = null;
                            }
                        }
                        if (XMLSpec.IsMatchingTag<RemoveSpec>(reader))
                        {
                            var isEmptyElement = reader.IsEmptyElement;
                            removeSpec = XMLSpec.Parse<RemoveSpec>(reader);

                            if (isEmptyElement)
                            {
                                featureSpec.Removes.Add(removeSpec);
                                removeSpec = null;
                            }
                        }
                        else if (XMLSpec.IsMatchingTag<Features.EnumSpec>(reader))
                        {
                            var enumSpec = XMLSpec.Parse<Features.EnumSpec>(reader);
                            requireSpec?.Enums.Add(enumSpec);
                            removeSpec?.Enums.Add(enumSpec);
                        }
                        else if (XMLSpec.IsMatchingTag<Features.CommandSpec>(reader))
                        {
                            var commandSpec = XMLSpec.Parse<Features.CommandSpec>(reader);
                            requireSpec?.Commands.Add(commandSpec);
                            removeSpec?.Commands.Add(commandSpec);
                        }
                        else if (XMLSpec.IsMatchingTag<Features.TypeSpec>(reader))
                        {
                            var typeSpec = XMLSpec.Parse<Features.TypeSpec>(reader);
                            requireSpec?.Types.Add(typeSpec);
                            removeSpec?.Types.Add(typeSpec);
                        }
                        break;
                    case XmlNodeType.EndElement:
                        if (XMLSpec.IsMatchingTag<FeatureSpec>(reader))
                        {
                            Spec.AddFeature(featureSpec);
                            return;
                        }
                        else if (XMLSpec.IsMatchingTag<Features.RequireSpec>(reader))
                        {
                            featureSpec.Requires.Add(requireSpec);
                            requireSpec = null;
                        }
                        else if (XMLSpec.IsMatchingTag<RemoveSpec>(reader))
                        {
                            featureSpec.Removes.Add(removeSpec);
                            removeSpec = null;
                        }
                        break;
                }
            }
        }

        private void ParseExtensions(XmlReader reader)
        {
            ExtensionSpec extensionSpec = null;
            Extensions.RequireSpec requireSpec = null;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (XMLSpec.IsMatchingTag<ExtensionSpec>(reader))
                        {
                            var isEmptyElement = reader.IsEmptyElement;
                            extensionSpec = XMLSpec.Parse<ExtensionSpec>(reader);

                            if (isEmptyElement)
                            {
                                Spec.AddExtension(extensionSpec);
                                extensionSpec = null;
                            }
                        }
                        else if (XMLSpec.IsMatchingTag<Extensions.RequireSpec>(reader))
                        {
                            var isEmptyElement = reader.IsEmptyElement;
                            requireSpec = XMLSpec.Parse<Extensions.RequireSpec>(reader);

                            if (isEmptyElement)
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
                            Spec.AddExtension(extensionSpec);
                            extensionSpec = null;
                        }
                        else if (XMLSpec.IsMatchingTag<Extensions.RequireSpec>(reader))
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
    }
}
