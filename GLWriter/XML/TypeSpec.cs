using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace GLWriter.XML
{
    public class TypeSpec : XMLSpec
    {
        public TypeSpec() : base("type") { }

        public string Name { get; set; }
        public string Requires { get; set; }
        public string Comment { get; set; }

        public override void SetAttribute(string name, string value)
        {
            switch (name)
            {
                case "name":
                    Name = value;
                    break;
                case "requires":
                    Requires = value;
                    break;
                case "comment":
                    Comment = value;
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
            }
        }
    }
}
