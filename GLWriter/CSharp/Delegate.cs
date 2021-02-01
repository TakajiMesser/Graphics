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
            builder.Append(ToCharacter(function.ReturnType, function.EnumName));
            builder.Append("_");

            foreach (var parameter in function.Parameters)
            {
                builder.Append(ToCharacter(parameter.DataType, parameter.EnumName));
            }

            Name = builder.ToString();
            ReturnType = function.ReturnType;
            EnumName = function.EnumName;

            for (var i = 0; i < function.Parameters.Count; i++)
            {
                var parameter = function.Parameters[i];

                Parameters.Add(new Parameter()
                {
                    Name = "v" + i,
                    DataType = parameter.DataType,
                    EnumName = parameter.EnumName
                });
            }
        }

        public string Name { get; }
        public DataTypes ReturnType { get; set; }
        public string EnumName { get; set; }
        public List<Parameter> Parameters { get; set; } = new List<Parameter>();

        private string ToText(DataTypes dataType, string enumName)
        {
            if (dataType == DataTypes.ENUM)
            {
                return "SpiceEngine.GLFWBindings.GL.Enums." + enumName;
            }
            else if (dataType == DataTypes.ENUMPTR)
            {
                return "SpiceEngine.GLFWBindings.GL.Enums." + enumName + "*";
            }
            else
            {
                return DataTypeExtensions.ToText(dataType);
            }
        }

        private string ToCharacter(DataTypes dataType, string enumName)
        {
            if (dataType == DataTypes.ENUM)
            {
                var builder = new StringBuilder();
                builder.Append("E");
                builder.Append(enumName);
                builder.Append("E");

                return builder.ToString();
            }
            else if (dataType == DataTypes.ENUMPTR)
            {
                var builder = new StringBuilder();
                builder.Append("Ep");
                builder.Append(enumName);
                builder.Append("Ep");

                return builder.ToString();
            }
            else
            {
                return DataTypeExtensions.ToCharacter(dataType);
            }
        }

        public string ToDefinitionLine()
        {
            var builder = new StringBuilder();

            builder.Append("private delegate ");
            builder.Append(ToText(ReturnType, EnumName));
            builder.Append(" ");
            builder.Append(Name);
            builder.Append("(");

            for (var i = 0; i < Parameters.Count; i++)
            {
                var parameter = Parameters[i];
                var parameterType = ToText(parameter.DataType, parameter.EnumName);

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
