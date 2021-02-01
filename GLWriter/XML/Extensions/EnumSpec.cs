namespace GLWriter.XML.Extensions
{
    public class EnumSpec : XMLSpec
    {
        public EnumSpec() : base("enum") { }

        public string Name { get; set; }

        public override void SetAttribute(string name, string value)
        {
            switch (name)
            {
                case "name":
                    Name = value;
                    break;
            }
        }
    }
}
