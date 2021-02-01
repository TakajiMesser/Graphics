using System.Collections.Generic;

namespace GLWriter.XML.Extensions
{
    public class ExtensionSpec : XMLSpec
    {
        public ExtensionSpec() : base("extension") { }

        public string Name { get; set; }
        public string Supported { get; set; }

        public List<RequireSpec> Requires { get; } = new List<RequireSpec>();

        public override void SetAttribute(string name, string value)
        {
            switch (name)
            {
                case "name":
                    Name = value;
                    break;
                case "supported":
                    Supported = value;
                    break;
            }
        }
    }
}
