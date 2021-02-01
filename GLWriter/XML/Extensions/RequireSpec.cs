using System.Collections.Generic;

namespace GLWriter.XML.Extensions
{
    public class RequireSpec : XMLSpec
    {
        public RequireSpec() : base("require") { }

        public string Comment { get; set; }

        public List<EnumSpec> Enums { get; } = new List<EnumSpec>();
        public List<CommandSpec> Commands { get; } = new List<CommandSpec>();

        public override void SetAttribute(string name, string value)
        {
            switch (name)
            {
                case "comment":
                    Comment = value;
                    break;
            }
        }
    }
}
