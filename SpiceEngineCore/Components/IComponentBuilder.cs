namespace SpiceEngineCore.Components
{
    public interface IComponentBuilder { }
    public interface IComponentBuilder<T> : IComponentBuilder where T : IComponent
    {
        T ToComponent(int entityID);
    }
}
