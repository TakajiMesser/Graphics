using SpiceEngine.Physics.Shapes;

namespace SpiceEngine.Physics.Bodies
{
    public enum BodyStates
    {
        Awake,
        Asleep
    }

    public interface IBody
    {
        int EntityID { get; }
        BodyStates State { get; set; }
        //IShape Shape { get; }

        bool IsMovable { get; }
        bool IsPhysical { get; }
    }
}
