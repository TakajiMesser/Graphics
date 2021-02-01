using System.Collections.Generic;

namespace GLWriter.XML
{
    public class EnumSpec
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Comment { get; set; }

        public List<string> GroupNames { get; } = new List<string>();

        public void SetAttribute(string name, string value)
        {
            switch (name)
            {
                case "name":
                    Name = value;
                    break;
                case "value":
                    Value = value;
                    break;
                case "group":
                    foreach (var groupName in value.Split(","))
                    {
                        GroupNames.Add(groupName);
                    }
                    break;
                case "comment":
                    Comment = value;
                    break;
            }
        }
    }
}
