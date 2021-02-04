namespace GLWriter.XML.Features
{
    public class TypeSpec : XMLSpec
    {
        public TypeSpec() : base("type") { }

        public string Name { get; set; }
        public string Comment { get; set; }

        public override void SetAttribute(string name, string value)
        {
            switch (name)
            {
                case "name":
                    Name = value;
                    break;
                case "comment":
                    Comment = value;
                    break;
            }
        }
    }
}
