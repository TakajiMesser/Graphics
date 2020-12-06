using SpiceEngineCore.Geometry.Vectors;
using SpiceEngineCore.Rendering.Matrices;

namespace SweetGraphicsCore.Rendering.Models
{
    public interface IModelShape
    {
        Vector3 GetAveragePosition();
        void CenterAround(Vector3 position);
        void Transform(Transform transform);
    }
}
