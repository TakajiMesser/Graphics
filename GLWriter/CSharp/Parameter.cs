using System;

namespace GLWriter.CSharp
{
    public struct Parameter : IEquatable<Parameter>
    {
        public Parameter(string name, CSharpType type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; set; }
        public CSharpType Type { get; set; }

        public Parameter Converted(DataTypes dataType) => new Parameter()
        {
            Name = Name,
            Type = new CSharpType(dataType, Type.Modifier, Type.Group, Type.IsOut)
        };

        public string ToName() => IsReservedKeyword() ? "@" + Name : Name;

        //abstract as base bool break byte case catch char checked class const continue decimal default delegate do double else enum event explicit extern false finally fixed float for foreach goto if implicit in int interface internal is lock long namespace new null object operator out override params private protected public readonly ref return sbyte sealed short sizeof stackalloc static string struct switch this throw true try typeof uint ulong unchecked unsafe ushort using virtual void volatile while
        private bool IsReservedKeyword() => Name == "ref"
            || Name == "string"
            || Name == "params"
            || Name == "base";

        public override int GetHashCode() => HashCode.Combine(Name, Type);

        public override bool Equals(object obj) => obj is Parameter type && Equals(type);

        public bool Equals(Parameter other) => Name == other.Name && Type == other.Type;

        public static bool operator ==(Parameter left, Parameter right) => left.Equals(right);

        public static bool operator !=(Parameter left, Parameter right) => !(left == right);
    }
}
