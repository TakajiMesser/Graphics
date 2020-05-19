using OpenTK;

namespace SpiceEngineCore.Physics
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
        bool IsPhysical { get; set; }

        Vector3 Position { get; set; }

        ICollision GetCollision(IBody body);
    }
}
