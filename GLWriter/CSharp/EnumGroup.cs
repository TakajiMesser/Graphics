using System.Collections.Generic;

namespace GLWriter.CSharp
{
    public class EnumGroup
    {
        public EnumGroup(string name) => Name = name;

        public string Name { get; }
        public List<EnumValue> Values { get; } = new List<EnumValue>();

        public IEnumerable<string> ToLines()
        {
            yield return "namespace SpiceEngine.GLFWBindings.GLEnums";
            yield return "{";
            yield return "\t" + "public enum " + Name;
            yield return "\t" + "{";

            foreach (var value in Values)
            {
                yield return "\t\t" + value.ToLine();
            }

            yield return "\t" + "}";
            yield return "}";
        }
    }
}
