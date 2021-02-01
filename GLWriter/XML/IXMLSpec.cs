namespace GLWriter.XML
{
    public interface IXMLSpec
    {
        string Tag { get; }
        string Content { get; }

        void SetContent(string value);
        void SetAttribute(string name, string value);
        void SetInnerElement(string name, string value);
    }
}
