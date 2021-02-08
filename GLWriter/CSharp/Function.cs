using GLWriter.Utilities;
using System.Collections.Generic;
using System.Text;

namespace GLWriter.CSharp
{
    public class Function
    {
        public Function(string name)
        {
            GLName = name;
            FieldName = "_" + name;

            if (name.StartsWith("gl"))
            {
                name = name.Substring("gl".Length);
            }

            Name = name.Capitalized();
        }

        public string Name { get; set; }
        public string GLName { get; }
        public string FieldName { get; }

        public CSharpType ReturnType { get; set; }
        public List<Parameter> Parameters { get; } = new List<Parameter>();

        public Delegate Delegate { get; set; }

        public string ToDefinitionLine()
        {
            var builder = new StringBuilder();
            builder.Append("public static ");
            builder.Append(ReturnType.ToText());
            builder.Append(" ");
            builder.Append(Name);
            builder.Append("(");

            for (var i = 0; i < Parameters.Count; i++)
            {
                if (i > 0)
                {
                    builder.Append(", ");
                }

                var parameter = Parameters[i];
                builder.Append(parameter.Type.ToText());
                builder.Append(" ");
                builder.Append(parameter.ToName());
            }

            builder.Append(") => ");
            builder.Append(FieldName);
            builder.Append("(");

            for (var i = 0; i < Parameters.Count; i++)
            {
                if (i > 0)
                {
                    builder.Append(", ");
                }

                builder.Append(Parameters[i].ToName());
            }

            builder.Append(");");
            return builder.ToString();
        }

        public string ToFieldLine()
        {
            var builder = new StringBuilder();
            builder.Append("private static ");
            builder.Append(Delegate.Name);
            builder.Append(" ");
            builder.Append(FieldName);
            builder.Append(";");

            return builder.ToString();
        }

        public string ToLoadLine()
        {
            var builder = new StringBuilder();
            builder.Append(FieldName);
            builder.Append(" = ");
            builder.Append("GetFunctionDelegate");
            builder.Append("<");
            builder.Append(Delegate.Name);
            builder.Append(">");
            builder.Append("(\"");
            builder.Append(GLName);
            builder.Append("\");");

            return builder.ToString();
        }
    }
}
