namespace SpiceEngine.Rendering.Vertices
{
    public interface IColorVertex : IVertex
    {
        Vector4 Color { get; }

        IColorVertex Colored(Vector4 color);
    }
}
