using GLWriter.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GLWriter.CSharp
{
    public class Overload
    {
        // TODO - Generate "easy" function overloads
        // Change uint parameters to int and explicitly cast for ease
        // Convert char* to string
        // If function has void return and last parameter is void* type, then convert to and return as byte[]
        private List<Parameter> _parameters = new List<Parameter>();
        private List<Conversion> _conversions = new List<Conversion>();
        private Conversion _returnConversion;
        
        public Overload(Function function, Overload overload = null)
        {
            Function = function;
            Name = function.Name;
            Call = overload;
        }

        public Function Function { get; }
        public Overload Call { get; }

        public string Name { get; set; }
        public CSharpType ReturnType { get; set; }

        public string BufferName { get; private set; }
        public CSharpType BufferType { get; private set; }

        public bool IsExpressionBodied { get; private set; }
        public bool IsUnsafe { get; private set; }
        public bool IsValid { get; private set; }
        public bool HasSuffixLines { get; private set; }

        private void ProcessConversion(Conversion conversion)
        {
            conversion.Process();

            if (conversion.RequiresMultipleLines)
            {
                IsExpressionBodied = false;
            }

            if (conversion.IsUnsafe)
            {
                IsUnsafe = true;
            }

            if (conversion.RequiresReturnLines)
            {
                HasSuffixLines = true;
            }
        }

        public void Process()
        {
            IsExpressionBodied = true;

            foreach (var conversion in _conversions)
            {
                ProcessConversion(conversion);
            }

            ProcessConversion(_returnConversion);

            if (_parameters.Count != Function.Parameters.Count)
            {
                IsValid = true;
            }
            else
            {
                for (var i = 0; i < _parameters.Count; i++)
                {
                    if (_parameters[i].Type != Function.Parameters[i].Type)
                    {
                        IsValid = true;
                        return;
                    }
                }

                if (ReturnType != Function.ReturnType)
                {
                    // We need to rename the original function to avoid a naming collision with this overload's signature
                    Function.Name += "Internal";
                    IsValid = true;
                    return;
                }

                IsValid = false;
            }
        }

        private static CSharpType TypeOverload(CSharpType type)
        {
            if (type.DataType == DataTypes.Char && type.Modifier == TypeModifiers.Pointer)
            {
                return new CSharpType(DataTypes.String, TypeModifiers.None, type.IsOut, type.Group);
            }
            else if (type.DataType == DataTypes.UInt && type.Modifier == TypeModifiers.None)
            {
                return new CSharpType(DataTypes.Int, TypeModifiers.None, type.IsOut, type.Group);
            }
            else if (type.DataType == DataTypes.UInt && type.Modifier == TypeModifiers.Pointer)
            {
                return new CSharpType(DataTypes.Int, TypeModifiers.Array);
            }
            else if (type.DataType == DataTypes.Int && type.Modifier == TypeModifiers.Pointer)
            {
                return new CSharpType(DataTypes.Int, TypeModifiers.Array);
            }
            else
            {
                return type;
            }
        }

        public static IEnumerable<Overload> ForFunction(Function function)
        {
            var a = 3;
            if (function.Name == "GenBuffers")
            {
                a = 4;
            }

            var typeOverload = new Overload(function)
            {
                ReturnType = TypeOverload(function.ReturnType)
            };

            typeOverload._returnConversion = new Conversion(function.ReturnType, typeOverload.ReturnType);

            foreach (var parameter in function.Parameters)
            {
                var overloadParameter = new Parameter(parameter.Name, TypeOverload(parameter.Type));

                typeOverload._parameters.Add(overloadParameter);
                typeOverload._conversions.Add(new Conversion(overloadParameter.Type, parameter.Type));
            }

            typeOverload.Process();

            var getterOverload = typeOverload.Getter();
            var singularOverload = getterOverload.Singular();

            if (typeOverload.IsValid)
            {
                yield return typeOverload;
            }

            if (getterOverload.IsValid)
            {
                yield return getterOverload;
            }

            if (singularOverload.IsValid)
            {
                yield return singularOverload;
            }
        }

        private Overload Getter()
        {
            var overload = new Overload(Function, this);

            // If the function has the "Gen" or "Get" prefix, has a VOID return, and the last parameter is an array, then we can return the array
            if (ReturnType.DataType == DataTypes.Void && ReturnType.Modifier == TypeModifiers.None && _parameters.Count > 0 && (Name.StartsWith("Gen") || Name.StartsWith("Get")))
            {
                string countName = null;

                for (var i = 0; i < _parameters.Count; i++)
                {
                    var parameter = _parameters[i];

                    if (i == 0)
                    {
                        if (parameter.Type.DataType == DataTypes.Int && parameter.Type.Modifier == TypeModifiers.None)
                        {
                            var overloadParameter = new Parameter(parameter.Name, TypeOverload(parameter.Type));

                            overload._parameters.Add(overloadParameter);
                            overload._conversions.Add(new Conversion(overloadParameter.Type, parameter.Type));

                            countName = parameter.Name;
                        }
                        else
                        {
                            return overload;
                        }
                    }
                    else if (i == _parameters.Count - 1)
                    {
                        if (parameter.Type.Modifier == TypeModifiers.Array)
                        {
                            overload.BufferName = parameter.Name;
                            overload.ReturnType = parameter.Type;
                            overload._returnConversion = new Conversion(ReturnType, overload.ReturnType);

                            overload._conversions.Add(new Conversion(new CSharpType(), parameter.Type)
                            {
                                ReferenceName = countName
                            });
                        }
                        else
                        {
                            return overload;
                        }
                    }
                    else
                    {
                        var overloadParameter = new Parameter(parameter.Name, TypeOverload(parameter.Type));

                        overload._parameters.Add(overloadParameter);
                        overload._conversions.Add(new Conversion(overloadParameter.Type, parameter.Type));
                    }
                }

                overload._returnConversion = new Conversion(ReturnType, overload.ReturnType);
                overload.Process();
            }

            var a = 3;
            if (overload.Name == "GenBuffers")
            {
                a = 4;
            }

            return overload;
        }

        private Overload Singular()
        {
            var overload = new Overload(Function, this);

            // If the function is plural, the first parameter is an int with name "n", and the last parameter is a pointer, singularize the function name and remove the first parameter
            if (Name.IsPlural() && _parameters.Count > 0)
            {
                overload.Name = Name.Singularized();

                for (var i = 0; i < _parameters.Count; i++)
                {
                    var parameter = _parameters[i];

                    if (i == 0)
                    {
                        if ((parameter.Name != "n" && parameter.Name != "count") || parameter.Type.DataType != DataTypes.Int || parameter.Type.Modifier != TypeModifiers.None
                            || (i == _parameters.Count - 1 && ReturnType.Modifier != TypeModifiers.Array))
                        {
                            return overload;
                        }

                        overload._conversions.Add(new Conversion(new CSharpType(), parameter.Type));
                    }
                    else if (i == _parameters.Count - 1)
                    {
                        if (parameter.Type.Modifier == TypeModifiers.Array)
                        {
                            var overloadParameter = new Parameter(parameter.Name, parameter.Type.ToUnptr());

                            overload.BufferName = parameter.Name;
                            overload._parameters.Add(overloadParameter);
                            overload._conversions.Add(new Conversion(overloadParameter.Type, parameter.Type));
                        }
                        else
                        {
                            return overload;
                        }
                    }
                    else
                    {
                        var overloadParameter = new Parameter(parameter.Name, TypeOverload(parameter.Type));

                        overload._parameters.Add(overloadParameter);
                        overload._conversions.Add(new Conversion(overloadParameter.Type, parameter.Type));
                    }
                }

                if (ReturnType.Modifier == TypeModifiers.Array)
                {
                    overload.ReturnType = ReturnType.ToUnptr();
                }
                else
                {
                    overload.ReturnType = ReturnType;
                }

                overload._returnConversion = new Conversion(ReturnType, overload.ReturnType);
                overload.Process();
            }

            return overload;
        }

        private string GetReturnLine()
        {
            var bodyBuilder = new StringBuilder();

            if (Call != null)
            {
                bodyBuilder.Append(Call.Name);
            }
            else
            {
                bodyBuilder.Append(Function.FieldName);
            }

            bodyBuilder.Append("(");

            for (var i = 0; i < _conversions.Count; i++)
            {
                if (i > 0)
                {
                    bodyBuilder.Append(", ");
                }

                var parameterName = i < _parameters.Count ? _parameters[i].ToName() : "";
                bodyBuilder.Append(_conversions[i].ToText(parameterName));
            }

            bodyBuilder.Append(")");

            return _returnConversion.ToText(bodyBuilder.ToString()) + ";";
        }

        public IEnumerable<string> ToDefinitionLines()
        {
            var builder = new StringBuilder();

            builder.Append("public static ");
            builder.Append(ReturnType.ToText());
            builder.Append(" ");
            builder.Append(Name);
            builder.Append("(");

            for (var i = 0; i < _parameters.Count; i++)
            {
                if (i > 0)
                {
                    builder.Append(", ");
                }

                var parameter = _parameters[i];

                builder.Append(parameter.Type.ToText());
                builder.Append(" ");
                builder.Append(parameter.ToName());
            }

            builder.Append(")");

            if (IsExpressionBodied)
            {
                builder.Append(" => ");
                builder.Append(GetReturnLine());

                yield return builder.ToString();
            }
            else
            {
                yield return builder.ToString();
                yield return "{";

                if (IsUnsafe)
                {
                    yield return "    unsafe";
                    yield return "    {";
                }

                var nTabs = IsUnsafe ? 2 : 1;

                for (var i = 0; i < _conversions.Count; i++)
                {
                    var conversion = _conversions[i];
                    var parameterName = i < _parameters.Count ? _parameters[i].ToName() : "";

                    if (conversion.RequiresMultipleLines)
                    {
                        foreach (var line in conversion.ToLines(parameterName, nTabs))
                        {
                            yield return line;
                        }

                        if (conversion.IsFixed)
                        {
                            nTabs++;
                        }
                    }
                }

                var returnBuilder = new StringBuilder();

                for (var i = 0; i < nTabs; i++)
                {
                    returnBuilder.Append("    ");
                }

                var a = 3;
                if (Name == "GenTextures")
                {
                    a = 4;
                }

                if (ReturnType.DataType == DataTypes.Void && ReturnType.Modifier == TypeModifiers.None)
                {
                    returnBuilder.Append(GetReturnLine());
                    yield return returnBuilder.ToString();
                }
                else
                {
                    var returnLine = GetReturnLine();

                    if (_returnConversion.RequiresMultipleLines)
                    {
                        foreach (var line in _returnConversion.ToLines(returnLine, nTabs))
                        {
                            yield return line;
                        }

                        if (_returnConversion.IsFixed)
                        {
                            nTabs++;
                        }
                    }
                    else if (HasSuffixLines)
                    {
                        returnBuilder.Append(GetReturnLine());
                        yield return returnBuilder.ToString();

                        foreach (var conversion in _conversions)
                        {
                            foreach (var line in conversion.ToReturnLines(nTabs))
                            {
                                yield return line;
                            }
                        }
                    }
                    else
                    {
                        returnBuilder.Append("return " + GetReturnLine());
                        yield return returnBuilder.ToString();
                    }
                }

                //returnBuilder.Append(ReturnType.DataType == DataTypes.Void && ReturnType.Modifier == TypeModifiers.None ? GetReturnLine() : "return " + GetReturnLine());
                //yield return returnBuilder.ToString();

                for (var i = nTabs - 1; i >= 0; i--)
                {
                    var linebuilder = new StringBuilder();

                    for (var j = 0; j < i; j++)
                    {
                        linebuilder.Append("    ");
                    }

                    linebuilder.Append("}");
                    yield return linebuilder.ToString();
                }
            }
        }

        // Original
        public unsafe static void DeleteBuffers(int n, uint* buffers) { }

        // Type Overload
        public static void DeleteBuffers(int n, int[] buffers)
        {
            unsafe
            {
                uint[] convertedBuffers = Array.ConvertAll(buffers, i => (uint)i);
                fixed (uint* buffersPtr = &convertedBuffers[0])
                {
                    DeleteBuffers(n, buffersPtr);
                }
            }
        }

        // Singular Overload
        public static void DeleteBuffer(int buffer)
        {
            unsafe
            {
                int[] buffers = new int[] { buffer };
                DeleteBuffers(1, buffers);
            }
        }

        // Original
        public unsafe static void GenBuffers(int n, uint* buffers) { }

        // Type Overload
        public static void GenBuffers(int n, int[] buffers)
        {
            unsafe
            {
                uint[] convertedBuffers = Array.ConvertAll(buffers, i => (uint)i);
                fixed (uint* buffersPtr = &convertedBuffers[0])
                {
                    GenBuffers(n, buffersPtr);
                }
            }
        }

        // Getter Overload
        public static int[] GenBuffers(int n)
        {
            unsafe
            {
                int[] buffers = new int[n];
                GenBuffers(n, buffers);
                return buffers;
            }
        }

        // Singular Overload
        public static int GenBuffer()
        {
            unsafe
            {
                var buffers = GenBuffers(1);
                return buffers[0];
            }
        }
    }
}
