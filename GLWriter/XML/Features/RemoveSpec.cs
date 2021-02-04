using System.Collections.Generic;

namespace GLWriter.XML.Features
{
    public class RemoveSpec : XMLSpec
    {
        public RemoveSpec() : base("remove") { }

        public string Profile { get; set; }
        public string Comment { get; set; }

        public List<EnumSpec> Enums { get; } = new List<EnumSpec>();
        public List<CommandSpec> Commands { get; } = new List<CommandSpec>();
        public List<TypeSpec> Types { get; } = new List<TypeSpec>();

        public override void SetAttribute(string name, string value)
        {
            switch (name)
            {
                case "profile":
                    Profile = value;
                    break;
                case "comment":
                    Comment = value;
                    break;
            }
        }
    }
}
