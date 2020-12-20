using OpenTK;

namespace SpiceEngineCore.Entities
{
    public interface ILight : IEntity
    {
        Vector4 Color { get; set; }
        float Intensity { get; set; }
    }
}
