namespace GLWriter.CSharp
{
    public class Parameter
    {
        public Parameter() { }
        public Parameter(DataTypes dataType, string name)
        {
            DataType = dataType;
            Name = name;
        }

        public string Name { get; set; }
        public DataTypes DataType { get; set; }
        public string Group { get; set; }
    }
}
