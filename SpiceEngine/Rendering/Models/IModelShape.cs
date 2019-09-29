using OpenTK;
using SpiceEngineCore.Rendering.Matrices;

namespace SpiceEngine.Rendering.Meshes
{
    public interface IModelShape
    {
        Vector3 GetAveragePosition();
        void CenterAround(Vector3 position);
        void Transform(Transform transform);
    }
}
