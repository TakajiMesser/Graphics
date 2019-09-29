using SpiceEngineCore.Entities;
using SpiceEngineCore.Inputs;
using SpiceEngineCore.Physics.Collisions;
using SpiceEngineCore.Scripting.StimResponse;
using SpiceEngineCore.Utilities;

namespace SpiceEngineCore.Scripting
{
    public interface IBehavior
    {
        BehaviorStatus Tick();

        void SetActor(IActor actor);
        void SetCamera(ICamera camera);
        void SetEntityProvider(IEntityProvider entityProvider);
        void SetCollisionProvider(ICollisionProvider collisionProvider);
        void SetInputProvider(IInputProvider inputProvider);
        void SetStimulusProvider(IStimulusProvider stimulusProvider);
        void SetProperty(string name, object value);
    }
}
