namespace GLWriter.XML
{
    public class ProtoSpec : XMLSpec
    {
        public ProtoSpec() : base("proto") { }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Group { get; set; }

        public override void SetAttribute(string name, string value)
        {
            switch (name)
            {
                case "group":
                    Group = value;
                    break;
            }
        }

        public override void SetInnerElement(string name, string value)
        {
            switch (name)
            {
                case "name":
                    Name = value;
                    break;
                case "ptype":
                    Type = value;
                    break;
            }
        }
    }
}
