namespace SpiceEngineCore.Rendering
{
    public struct Uniform<T> : IUniform// where T : struct
    {
        public Uniform(string name, T value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public T Value { get; }
    }
}
