using System.Collections.Generic;
using System.Text;

namespace GLWriter.CSharp
{
    public class Delegate
    {
        public Delegate(Function function)
        {
            var builder = new StringBuilder();
            builder.Append("DEL");
            builder.Append("_");
            builder.Append(function.ReturnType.ToCode());
            builder.Append("_");

            foreach (var parameter in function.Parameters)
            {
                builder.Append(parameter.Type.ToCode());
            }

            Name = builder.ToString();
            ReturnType = function.ReturnType;

            for (var i = 0; i < function.Parameters.Count; i++)
            {
                var parameter = function.Parameters[i];

                Parameters.Add(new Parameter()
                {
                    Name = "v" + i,
                    Type = parameter.Type
                });
            }
        }

        public string Name { get; }
        public CSharpType ReturnType { get; set; }
        public List<Parameter> Parameters { get; set; } = new List<Parameter>();

        public string ToDefinitionLine()
        {
            var builder = new StringBuilder();

            builder.Append("private delegate ");
            builder.Append(ReturnType.ToText());
            builder.Append(" ");
            builder.Append(Name);
            builder.Append("(");

            for (var i = 0; i < Parameters.Count; i++)
            {
                var parameter = Parameters[i];
                var parameterType = parameter.Type.ToText();

                if (i > 0)
                {
                    builder.Append(", ");
                }

                builder.Append(parameterType);
                builder.Append(" ");
                builder.Append(parameter.Name);
            }

            builder.Append(");");

            return builder.ToString();
        }
    }
}
