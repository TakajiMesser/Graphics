using GLWriter.Utilities;
using System;
using System.Text;

namespace GLWriter.CSharp
{
    public enum TypeModifiers
    {
        None,
        Pointer,
        DoublePointer,
        Array
    }

    public struct CSharpType : IEquatable<CSharpType>
    {
        public CSharpType(DataTypes dataType, TypeModifiers modifier = TypeModifiers.None, string group = null, bool isOut = false)
        {
            DataType = dataType;
            Modifier = modifier;
            Group = group;
            IsOut = isOut;
        }

        public DataTypes DataType { get; set; }
        public TypeModifiers Modifier { get; set; }
        public string Group { get; set; }
        public bool IsOut { get; set; }

        public bool IsPtr => Modifier == TypeModifiers.Pointer || DataType == DataTypes.String;

        public CSharpType ToPtr() => new CSharpType()
        {
            DataType = DataType,
            Modifier = TypeModifiers.Pointer,
            Group = Group,
            IsOut = IsOut
        };

        public CSharpType ToArray() => new CSharpType()
        {
            DataType = DataType,
            Modifier = TypeModifiers.Array,
            Group = Group,
            IsOut = IsOut
        };

        public CSharpType ToUnptr() => new CSharpType()
        {
            DataType = DataType,
            Modifier = TypeModifiers.None,
            Group = Group,
            IsOut = IsOut
        };

        public CSharpType Capitalized() => new CSharpType(DataType, Modifier, Group.Capitalized(), IsOut);

        public string ToText()
        {
            var builder = new StringBuilder();

            if (IsOut)
            {
                builder.Append("out ");
            }

            if (DataType == DataTypes.Enum)
            {
                builder.Append("SpiceEngine.GLFWBindings.GLEnums." + Group);
            }
            else if (DataType == DataTypes.Struct)
            {
                builder.Append("SpiceEngine.GLFWBindings.GLStructs." + Group);
            }
            else
            {
                builder.Append(DataType.ToText());
            }

            if (Modifier == TypeModifiers.Pointer)
            {
                builder.Append("*");
            }
            else if (Modifier == TypeModifiers.DoublePointer)
            {
                builder.Append("**");
            }
            else if (Modifier == TypeModifiers.Array)
            {
                builder.Append("[]");
            }

            return builder.ToString();
        }

        public string ToCode()
        {
            var builder = new StringBuilder();
            var codeBuilder = new StringBuilder(DataType.ToCode());

            if (Modifier == TypeModifiers.Pointer)
            {
                codeBuilder.Append("p");
            }
            else if (Modifier == TypeModifiers.DoublePointer)
            {
                codeBuilder.Append("pp");
            }
            else if (Modifier == TypeModifiers.Array)
            {
                codeBuilder.Append("a");
            }

            var code = codeBuilder.ToString();

            if (DataType == DataTypes.Enum || DataType == DataTypes.Struct)
            {
                builder.Append(code + Group + code);
            }
            else
            {
                builder.Append(code);
            }

            return builder.ToString();
        }

        public override int GetHashCode() => HashCode.Combine(DataType, Modifier, IsOut, Group);

        public override bool Equals(object obj) => obj is CSharpType type && Equals(type);

        public bool Equals(CSharpType other) => DataType == other.DataType
            && Modifier == other.Modifier
            && IsOut == other.IsOut
            && Group == other.Group;

        public static bool operator ==(CSharpType left, CSharpType right) => left.Equals(right);

        public static bool operator !=(CSharpType left, CSharpType right) => !(left == right);
    }
}
