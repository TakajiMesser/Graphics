using System.Collections.Generic;

namespace GLWriter.XML.Commands
{
    public class CommandSpec : XMLSpec
    {
        public CommandSpec() : base("command") { }

        public ProtoSpec Prototype { get; set; }
        public List<ParamSpec> Parameters { get; } = new List<ParamSpec>();
    }
}
