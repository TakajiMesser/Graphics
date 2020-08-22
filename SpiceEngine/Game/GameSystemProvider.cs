using SavoryPhysicsCore;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Game;
using SpiceEngineCore.Rendering;
using StarchUICore;
using TangyHIDCore;

namespace SpiceEngine.Game
{
    public class GameSystemProvider : SystemProvider
    {
        public IEntityProvider EntityProvider
        {
            get => GetEntityProvider();
            set => SetEntityProvider(value);
        }

        public IRenderProvider RenderProvider
        {
            get => GetRenderProvider();
            set => SetRenderProvider(value);
        }

        public IInputProvider InputProvider
        {
            get => GetGameSystem<IInputProvider>();
            set => AddGameSystem(value);
        }

        public IUIProvider UIProvider
        {
            get => GetGameSystem<IUIProvider>();
            set
            {
                AddGameSystem(value);
                AddComponentProvider<IElement>(value);
            }
        }

        public IPhysicsProvider PhysicsProvider
        {
            get => GetGameSystem<IPhysicsProvider>();
            set
            {
                AddGameSystem(value);
                AddComponentProvider<IBody>(value);
            }
        }
    }
}
