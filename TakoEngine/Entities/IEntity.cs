using OpenTK;

namespace TakoEngine.Entities
{
    public interface IEntity
    {
        int ID { get; set; }
        Vector3 Position { get; set; }
        Quaternion Rotation { get; set; }
        Vector3 Scale { get; set; }
    }
}
