using SpiceEngineCore.Components;

namespace SavoryPhysicsCore
{
    public interface IBodyBuilder : IComponentBuilder<IBody>
    {
        bool IsPhysical { get; }
    }
}
