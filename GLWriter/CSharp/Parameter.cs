namespace GLWriter.CSharp
{
    public class Parameter
    {
        public Parameter() { }
        public Parameter(string name, CSharpType type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; set; }
        public CSharpType Type { get; set; }

        public string ToName() => IsReservedKeyword() ? "@" + Name : Name;

        //abstract as base bool break byte case catch char checked class const continue decimal default delegate do double else enum event explicit extern false finally fixed float for foreach goto if implicit in int interface internal is lock long namespace new null object operator out override params private protected public readonly ref return sbyte sealed short sizeof stackalloc static string struct switch this throw true try typeof uint ulong unchecked unsafe ushort using virtual void volatile while
        private bool IsReservedKeyword() => Name == "ref"
            || Name == "string"
            || Name == "params"
            || Name == "base";
    }
}
