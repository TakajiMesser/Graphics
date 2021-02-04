using System.Collections.Generic;

namespace GLWriter.XML.Features
{
    public class FeatureSpec : XMLSpec
    {
        public FeatureSpec() : base("feature") { }

        public string Name { get; set; }
        public string API { get; set; }
        public string Number { get; set; }

        public List<RequireSpec> Requires { get; } = new List<RequireSpec>();
        public List<RemoveSpec> Removes { get; } = new List<RemoveSpec>();

        public Version Version => new Version(API, Number);

        public override void SetAttribute(string name, string value)
        {
            switch (name)
            {
                case "name":
                    Name = value;
                    break;
                case "api":
                    API = value;
                    break;
                case "number":
                    Number = value;
                    break;
            }
        }
    }
}
