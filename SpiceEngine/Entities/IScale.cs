using OpenTK;

namespace SpiceEngine.Entities
{
    public interface IScale
    {
        Vector3 Scale { get; set; }

        void ScaleBy(Vector3 scale);
    }
}
