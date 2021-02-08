using System.Collections.Generic;

namespace GLWriter.CSharp
{
    public class Struct
    {
        public Struct(string name) => Name = name;

        public string Name { get; }

        public IEnumerable<string> ToLines()
        {
            yield return "namespace SpiceEngine.GLFWBindings.GLStructs";
            yield return "{";
            yield return "\t" + "public struct " + Name + " { }";
            yield return "}";
        }
    }
}
