namespace GLWriter
{
    public class Parameter
    {
        public Parameter(DataTypes dataType, string name)
        {
            DataType = dataType;
            Name = name;
        }

        public DataTypes DataType { get; set; }
        public string Name { get; set; }
    }
}
