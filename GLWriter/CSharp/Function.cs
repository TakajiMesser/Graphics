using System.Collections.Generic;
using System.Text;

namespace GLWriter.CSharp
{
    public class Function
    {
        public string Name { get; set; }
        public DataTypes ReturnType { get; set; }
        public string Group { get; set; }
        public List<Parameter> Parameters { get; } = new List<Parameter>();

        public Delegate Delegate { get; set; }

        public string GetFunctionName()
        {
            var functionName = Name;

            if (functionName.StartsWith("gl"))
            {
                functionName = functionName.Substring("gl".Length);
            }

            var builder = new StringBuilder();

            for (var i = 0; i < functionName.Length; i++)
            {
                builder.Append(i == 0 ? char.ToUpper(functionName[i]) : functionName[i]);
            }

            return builder.ToString();
        }

        public string GetFieldName() => "_" + Name;

        private string ToName(Parameter parameter) => IsReservedKeyword(parameter.Name)
            ? "@" + parameter.Name
            : parameter.Name;

        //abstract as base bool break byte case catch char checked class const continue decimal default delegate do double else enum event explicit extern false finally fixed float for foreach goto if implicit in int interface internal is lock long namespace new null object operator out override params private protected public readonly ref return sbyte sealed short sizeof stackalloc static string struct switch this throw true try typeof uint ulong unchecked unsafe ushort using virtual void volatile while
        private bool IsReservedKeyword(string text) => text == "ref"
            || text == "string"
            || text == "params"
            || text == "base";
        
        public bool RequiresOverload()
        {
            if (ReturnType == DataTypes.CHARPTR || ReturnType == DataTypes.UINT)
            {
                return true;
            }

            foreach (var parameter in Parameters)
            {
                if (parameter.DataType == DataTypes.CHARPTR || parameter.DataType == DataTypes.UINT)
                {
                    return true;
                }
            }

            return false;
        }

        public bool RequiresMultiLineOverload()
        {
            if (ReturnType == DataTypes.CHARPTR)
            {
                return true;
            }

            foreach (var parameter in Parameters)
            {
                if (parameter.DataType == DataTypes.CHARPTR)
                {
                    return true;
                }
            }

            return false;
        }

        private DataTypes ConvertDataTypeForOverload(DataTypes dataType)
        {
            if (dataType == DataTypes.CHARPTR)
            {
                return DataTypes.STRING;
            }
            else if (dataType == DataTypes.UINT)
            {
                return DataTypes.INTEGER;
            }

            return dataType;
        }

        public IEnumerable<string> ToOverloadDefinitionLines()
        {
            var builder = new StringBuilder();
            builder.Append("public static ");

            var convertedReturnType = ConvertDataTypeForOverload(ReturnType);
            builder.Append(DataTypeExtensions.ToText(convertedReturnType, Group));

            builder.Append(" ");
            builder.Append(GetFunctionName());
            builder.Append("(");

            for (var i = 0; i < Parameters.Count; i++)
            {
                if (i > 0)
                {
                    builder.Append(", ");
                }

                var parameter = Parameters[i];
                var convertedParameterType = ConvertDataTypeForOverload(parameter.DataType);
                builder.Append(DataTypeExtensions.ToText(convertedParameterType, parameter.Group));

                builder.Append(" ");
                builder.Append(ToName(parameter));
            }

            builder.Append(")");

            if (RequiresMultiLineOverload())
            {
                yield return builder.ToString();
            }
            else
            {
                builder.Append(" => ");

                if (ReturnType == DataTypes.UINT)
                {
                    builder.Append("(int)");
                }

                builder.Append(GetFieldName());
                builder.Append("(");

                for (var i = 0; i < Parameters.Count; i++)
                {
                    if (i > 0)
                    {
                        builder.Append(", ");
                    }

                    var parameter = Parameters[i];
                    var parameterName = ToName(parameter);
                    var parameterType = Delegate.Parameters[i].DataType;

                    if (parameter.DataType == DataTypes.UINT)
                    {
                        builder.Append("(uint)");
                    }

                    builder.Append(parameterName);

                    if (parameter.DataType == DataTypes.INTPTR && parameterType == DataTypes.VOIDPTR)
                    {
                        builder.Append(parameterName + ".ToPointer()");
                    }
                    else if ((parameter.DataType == DataTypes.INTEGER || parameter.DataType == DataTypes.LONG) && parameterType == DataTypes.INTPTR)
                    {
                        builder.Append("new IntPtr(" + parameterName + ")");
                    }
                    else if (parameter.DataType == DataTypes.UINT && parameterType == DataTypes.UINTPTR)
                    {
                        builder.Append("&" + parameterName);
                    }
                    else
                    {
                        builder.Append(parameterName);
                    }
                }

                builder.Append(");");
                yield return builder.ToString();
            }

            // TODO - Generate "easy" function overloads
            // Change uint parameters to int and explicitly cast for ease
            // Convert char* to string
            // If function has void return and last parameter is void* type, then convert to and return as byte[]
        }

        public string ToDefinitionLine()
        {
            var builder = new StringBuilder();
            builder.Append("public static ");
            builder.Append(DataTypeExtensions.ToText(ReturnType, Group));
            builder.Append(" ");
            builder.Append(GetFunctionName());
            builder.Append("(");

            for (var i = 0; i < Parameters.Count; i++)
            {
                if (i > 0)
                {
                    builder.Append(", ");
                }

                var parameter = Parameters[i];
                var parameterType = DataTypeExtensions.ToText(parameter.DataType, parameter.Group);
                builder.Append(parameterType);
                builder.Append(" ");
                builder.Append(ToName(parameter));
            }

            builder.Append(") => ");
            builder.Append(GetFieldName());
            builder.Append("(");

            for (var i = 0; i < Parameters.Count; i++)
            {
                if (i > 0)
                {
                    builder.Append(", ");
                }

                var parameter = Parameters[i];
                var parameterName = ToName(parameter);
                var parameterType = Delegate.Parameters[i].DataType;

                if (parameter.DataType == DataTypes.INTPTR && parameterType == DataTypes.VOIDPTR)
                {
                    builder.Append(parameterName + ".ToPointer()");
                }
                else if ((parameter.DataType == DataTypes.INTEGER || parameter.DataType == DataTypes.LONG) && parameterType == DataTypes.INTPTR)
                {
                    builder.Append("new IntPtr(" + parameterName + ")");
                }
                else if (parameter.DataType == DataTypes.UINT && parameterType == DataTypes.UINTPTR)
                {
                    builder.Append("&" + parameterName);
                }
                else
                {
                    builder.Append(parameterName);
                }
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
            builder.Append(GetFieldName());
            builder.Append(";");

            return builder.ToString();
        }

        public string ToLoadLine()
        {
            var builder = new StringBuilder();
            builder.Append(GetFieldName());
            builder.Append(" = ");
            builder.Append("GetFunctionDelegate");
            builder.Append("<");
            builder.Append(Delegate.Name);
            builder.Append(">");
            builder.Append("(\"");
            builder.Append(Name);
            builder.Append("\");");

            return builder.ToString();
        }
    }
}
