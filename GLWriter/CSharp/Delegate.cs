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
            builder.Append(DataTypeExtensions.ToCharacter(function.ReturnType, function.Group));
            builder.Append("_");

            foreach (var parameter in function.Parameters)
            {
                builder.Append(DataTypeExtensions.ToCharacter(parameter.DataType, parameter.Group));
            }

            Name = builder.ToString();
            ReturnType = function.ReturnType;
            Group = function.Group;

            for (var i = 0; i < function.Parameters.Count; i++)
            {
                var parameter = function.Parameters[i];

                Parameters.Add(new Parameter()
                {
                    Name = "v" + i,
                    DataType = parameter.DataType,
                    Group = parameter.Group
                });
            }
        }

        public string Name { get; }
        public DataTypes ReturnType { get; set; }
        public string Group { get; set; }
        public List<Parameter> Parameters { get; set; } = new List<Parameter>();

        public string ToDefinitionLine()
        {
            var builder = new StringBuilder();

            builder.Append("private delegate ");
            builder.Append(DataTypeExtensions.ToText(ReturnType, Group));
            builder.Append(" ");
            builder.Append(Name);
            builder.Append("(");

            for (var i = 0; i < Parameters.Count; i++)
            {
                var parameter = Parameters[i];
                var parameterType = DataTypeExtensions.ToText(parameter.DataType, parameter.Group);

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
