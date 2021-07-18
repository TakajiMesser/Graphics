namespace SpiceEngineCore.Components
{
    public abstract class Component : IComponent
    {
        public Component(int entityID) => EntityID = entityID;

        public int EntityID { get; }
    }
}
