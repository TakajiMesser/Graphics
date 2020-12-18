using OpenTK;

namespace SpiceEngineCore.Components
{
    public interface IComponentBuilder { }
    public interface IComponentBuilder<T> : IComponentBuilder where T : IComponent
    {
        Vector3 Position { get; set; }

        T ToComponent(int entityID);
    }
}
