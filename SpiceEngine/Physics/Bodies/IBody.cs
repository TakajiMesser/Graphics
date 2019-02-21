using OpenTK;
using SpiceEngine.Physics.Shapes;

namespace SpiceEngine.Physics.Bodies
{
    public interface IBody
    {
        int EntityID { get; }
        IShape Shape { get; }
    }
}
