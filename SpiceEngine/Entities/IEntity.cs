using OpenTK;

namespace SpiceEngine.Entities
{
    public interface IEntity
    {
        int ID { get; set; }
        Vector3 Position { get; set; }
        //IEntity Duplicate();
    }
}
