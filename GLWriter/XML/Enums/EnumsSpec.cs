using System.Collections.Generic;

namespace GLWriter.XML.Enums
{
    public class EnumsSpec
    {
        public string NameSpace { get; set; }
        public string Group { get; set; }
        public string Vendor { get; set; }
        public string Type { get; set; }
        public string Comments { get; set; }

        public List<EnumSpec> ChildSpecs { get; } = new List<EnumSpec>();

        public void SetAttribute(string name, string value)
        {
            switch (name)
            {
                case "namespace":
                    NameSpace = value;
                    break;
                case "group":
                    Group = value;
                    break;
                case "type":
                    Type = value;
                    break;
                case "vendor":
                    Vendor = value;
                    break;
                case "comments":
                    Comments = value;
                    break;
            }
        }
    }
}
