namespace GLWriter.XML
{
    public class ParamSpec : XMLSpec
    {
        public ParamSpec() : base("param") { }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Group { get; set; }
        public string Class { get; set; }
        public string Length { get; set; }

        public override void SetAttribute(string name, string value)
        {
            switch (name)
            {
                case "group":
                    Group = value;
                    break;
                case "class":
                    Class = value;
                    break;
                case "len":
                    Length = value;
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
