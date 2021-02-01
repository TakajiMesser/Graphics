using System.Xml;

namespace GLWriter.XML
{
    public abstract class XMLSpec : IXMLSpec
    {
        public XMLSpec(string tag) => Tag = tag;

        public string Tag { get; }
        public string Content { get; private set; }

        public void SetContent(string value) => Content = value;

        public virtual void SetAttribute(string name, string value) { }
        public virtual void SetInnerElement(string name, string value) { }

        public static bool IsMatchingTag<T>(XmlReader reader) where T : XMLSpec, new()
        {
            var spec = new T();
            return reader.Name == spec.Tag;
        }

        public static T Parse<T>(XmlReader reader) where T : XMLSpec, new()
        {
            var spec = new T();

            if (reader.AttributeCount > 0)
            {
                for (var i = 0; i < reader.AttributeCount; i++)
                {
                    reader.MoveToAttribute(i);
                    spec.SetAttribute(reader.Name, reader.Value);
                }

                //reader.Read();
            }

            /*if (!reader.IsEmptyElement)
            {
                if (reader.NodeType == XmlNodeType.Text)
                {
                    spec.SetContent(reader.Value);
                }
                else if (reader.NodeType == XmlNodeType.Element)
                {
                    spec.SetContent(reader.ReadElementContentAsString());
                }
            }*/

            return spec;
        }

        public static void ParseInnerElement<T>(T spec, string name, string value) where T : XMLSpec
        {
            if (spec != null)
            {
                spec.Content += value;

                if (name != spec.Tag)
                {
                    spec.SetInnerElement(name, value);
                }
            }
        }
    }
}
