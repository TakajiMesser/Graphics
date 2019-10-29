using OpenTK;
using SpiceEngineCore.Entities;

namespace SpiceEngineCore.Game.Loading.Builders
{
    public interface IEntityBuilder
    {
        Vector3 Position { get; set; }

        IEntity ToEntity();
    }
}
