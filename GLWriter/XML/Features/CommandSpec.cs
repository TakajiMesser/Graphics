namespace GLWriter.XML.Features
{
    public class CommandSpec : XMLSpec
    {
        public CommandSpec() : base("command") { }

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
