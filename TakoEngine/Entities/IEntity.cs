using OpenTK;

namespace TakoEngine.Entities
{
    public interface IEntity
    {
        int ID { get; set; }
        Vector3 Position { get; set; }
    }
}
