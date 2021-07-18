namespace SpiceEngineCore.Rendering
{
    public interface IRenderContextProvider
    {
        IRenderContext CurrentContext { get; }
    }
}
