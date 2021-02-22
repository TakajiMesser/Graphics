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
        public bool NeedsExplicitReturn { get; private set; }
        public bool HasSuffixLines { get; private set; }

        public bool IsDuplicateOf(Function function)
        {
            if (Name != function.Name || ReturnType != function.ReturnType || _parameters.Count != function.Parameters.Count)
            {
                return false;
            }

            for (var i = 0; i < _parameters.Count; i++)
            {
                if (_parameters[i] != function.Parameters[i])
                {
                    return false;
                }
            }

            return true;
        }

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

            if (conversion.ContainsReturn)
            {
                NeedsExplicitReturn = false;
            }
        }

        public void Process()
        {
            IsExpressionBodied = true;
            NeedsExplicitReturn = true;

            foreach (var conversion in _conversions)
            {
                ProcessConversion(conversion);
            }

            ProcessConversion(_returnConversion);

            if (ReturnType.DataType == DataTypes.Void && ReturnType.Modifier == TypeModifiers.None)
            {
                NeedsExplicitReturn = false;
            }

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

        private static CSharpType TypeOverload(CSharpType type) => (type.DataType, type.Modifier) switch
        {
            (DataTypes.Char, TypeModifiers.Pointer) => new CSharpType(DataTypes.String, TypeModifiers.None, type.Group, type.IsOut),
            (DataTypes.UInt, TypeModifiers.None) => new CSharpType(DataTypes.Int, TypeModifiers.None, type.Group, type.IsOut),
            (DataTypes.UInt, TypeModifiers.Pointer) => new CSharpType(DataTypes.Int, TypeModifiers.Array, type.Group, type.IsOut),
            (DataTypes.Int, TypeModifiers.Pointer) => new CSharpType(DataTypes.Int, TypeModifiers.Array, type.Group, type.IsOut),
            (DataTypes.Float, TypeModifiers.Pointer) => new CSharpType(DataTypes.Float, TypeModifiers.Array, type.Group, type.IsOut),
            (DataTypes.Enum, TypeModifiers.Pointer) => new CSharpType(DataTypes.Enum, TypeModifiers.Array, type.Group, type.IsOut),
            (DataTypes.Char, TypeModifiers.DoublePointer) => new CSharpType(DataTypes.String, TypeModifiers.Array, type.Group, type.IsOut),
            (DataTypes.Void, TypeModifiers.Pointer) => new CSharpType(DataTypes.IntPtr, TypeModifiers.None, type.Group, type.IsOut),
            _ => type
        };

        public static IEnumerable<Overload> ForFunction(Function function)
        {
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
            var singularOverload = getterOverload.IsValid ? getterOverload.Singular() : typeOverload.Singular();

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
            /*var a = 3;
            if (Function.Name == "GetFloatv")
            {
                a = 4;
            }*/

            var overload = new Overload(Function, this);

            // If the function has the "Gen" or "Get" prefix, has a VOID return, and the last parameter is an array, then we can return the array
            if (ReturnType.DataType == DataTypes.Void && ReturnType.Modifier == TypeModifiers.None && _parameters.Count > 0 && (Name.StartsWith("Gen") || Name.StartsWith("Get")))
            {
                string countName = null;

                for (var i = 0; i < _parameters.Count; i++)
                {
                    var parameter = _parameters[i];

                    if (parameter.Type.DataType == DataTypes.Int && parameter.Type.Modifier == TypeModifiers.None && (parameter.Name == "n" || parameter.Name == "count"))
                    {
                        countName = parameter.Name;
                    }

                    if (i == _parameters.Count - 1)
                    {
                        if (parameter.Type.Modifier == TypeModifiers.Array)
                        {
                            overload.BufferName = parameter.Name;
                            overload.ReturnType = parameter.Type;
                            overload._returnConversion = new Conversion(ReturnType, overload.ReturnType);

                            overload._conversions.Add(new Conversion(new CSharpType(), parameter.Type)
                            {
                                ReferenceName = countName ?? "1"
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

            return overload;
        }

        // TODO - Check if this overload signature already exists in the set of defined functions (e.g. glDrawBuffer)
        private Overload Singular()
        {
            var a = 3;
            if (Name == "GenFrameBuffers")
            {
                a = 4;
            }

            var overload = new Overload(Function, this);

            // If the function is plural, the first parameter is an int with name "n", and the last parameter is a pointer, singularize the function name and remove the first parameter
            if (Name.IsPlural() && _parameters.Count > 0)
            {
                var nPluralParameters = 0;
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
                    else
                    {
                        if (parameter.Type.Modifier == TypeModifiers.Array && parameter.Name.IsPlural())
                        {
                            nPluralParameters++;
                            var overloadParameter = new Parameter(parameter.Name.Singularized(), parameter.Type.ToUnptr());

                            overload.BufferName = parameter.Name;
                            overload._parameters.Add(overloadParameter);
                            overload._conversions.Add(new Conversion(overloadParameter.Type, parameter.Type));
                        }
                        else
                        {
                            var overloadParameter = new Parameter(parameter.Name, TypeOverload(parameter.Type));

                            overload._parameters.Add(overloadParameter);
                            overload._conversions.Add(new Conversion(overloadParameter.Type, parameter.Type));
                        }
                    }
                }

                if (nPluralParameters == 0)
                {
                    //return overload;
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

            var parameterIndex = 0;

            for (var i = 0; i < _conversions.Count; i++)
            {
                if (i > 0)
                {
                    bodyBuilder.Append(", ");
                }

                var conversion = _conversions[i];
                var parameterName = "";

                if (conversion.FromType.DataType != DataTypes.None && parameterIndex < _parameters.Count)
                {
                    parameterName = _parameters[parameterIndex].ToName();
                    parameterIndex++;
                }

                bodyBuilder.Append(conversion.ToText(parameterName));
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
                var parameterIndex = 0;

                for (var i = 0; i < _conversions.Count; i++)
                {
                    var conversion = _conversions[i];
                    var parameterName = "";

                    if (conversion.FromType.DataType != DataTypes.None && parameterIndex < _parameters.Count)
                    {
                        parameterName = _parameters[parameterIndex].ToName();
                        parameterIndex++;
                    }

                    if (conversion.RequiresMultipleLines)
                    {
                        foreach (var line in conversion.ToPrefixLines(parameterName, nTabs))
                        {
                            yield return line;
                        }

                        if (conversion.IsFixed)
                        {
                            nTabs++;
                        }
                    }
                }

                var a = 3;
                if (Name == "GenBuffer" || Name == "PrioritizeTexture")
                {
                    a = 4;
                }

                var returnBuilder = new StringBuilder();

                for (var i = 0; i < nTabs; i++)
                {
                    returnBuilder.Append("    ");
                }

                if (_returnConversion.RequiresMultipleLines)
                {
                    foreach (var line in _returnConversion.ToPrefixLines(GetReturnLine(), nTabs))
                    {
                        yield return line;
                    }

                    if (_returnConversion.IsFixed)
                    {
                        //nTabs++;
                    }
                }

                if (Name == "GenBuffer" || Name == "PrioritizeTexture")
                {
                    a = 4;
                }

                if (NeedsExplicitReturn)
                {
                    returnBuilder.Append("return " + GetReturnLine());
                    yield return returnBuilder.ToString();
                }
                else if (!_returnConversion.ContainsReturn)
                {
                    returnBuilder.Append(GetReturnLine());
                    yield return returnBuilder.ToString();
                }

                if (HasSuffixLines)
                {
                    parameterIndex = _parameters.Count - 1;

                    for (var i = _conversions.Count - 1; i >= 0; i--)
                    {
                        var conversion = _conversions[i];
                        var parameterName = "";

                        if (conversion.FromType.DataType != DataTypes.None && parameterIndex >= 0)
                        {
                            parameterName = _parameters[parameterIndex].ToName();
                            parameterIndex--;
                        }

                        if (conversion.RequiresMultipleLines)
                        {
                            if (conversion.IsFixed)
                            {
                                nTabs--;
                            }

                            foreach (var line in conversion.ToSuffixLines(parameterName, nTabs))
                            {
                                yield return line;
                            }
                        }
                    }
                }

                //returnBuilder.Append(ReturnType.DataType == DataTypes.Void && ReturnType.Modifier == TypeModifiers.None ? GetReturnLine() : "return " + GetReturnLine());
                //yield return returnBuilder.ToString();

                if (IsUnsafe)
                {
                    yield return "    }";
                }

                yield return "}";

                /*for (var i = nTabs - 1; i >= 0; i--)
                {
                    var linebuilder = new StringBuilder();

                    for (var j = 0; j < i; j++)
                    {
                        linebuilder.Append("    ");
                    }

                    linebuilder.Append("}");
                    yield return linebuilder.ToString();
                }*/
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
