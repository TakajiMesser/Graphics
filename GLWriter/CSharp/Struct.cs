using System.Collections.Generic;

namespace GLWriter.CSharp
{
    public class Struct
    {
        public Struct(string name) => Name = name;

        public string Name { get; }

        public IEnumerable<string> ToLines()
        {
            yield return "using System;";
            yield return "";
            yield return "namespace SpiceEngine.GLFWBindings.GLStructs";
            yield return "{";
            yield return "\t" + "public struct " + Name + " { }";
            yield return "}";
        }
    }
}
