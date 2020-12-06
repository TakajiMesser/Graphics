using System;

namespace SpiceEngineCore.Rendering
{
    public interface IUniform
    {
        string Name { get; }
    }

    public interface IUniform<T> : IUniform
    {
        T Value { get; }
    }
}
