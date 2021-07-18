using SpiceEngineCore.Commands;

namespace SpiceEngineCore.Game
{
    public interface IGameSystem : ITick, IUpdate
    {
        int TickRate { get; set; }

        void SetSystemProvider(ISystemProvider systemProvider);
        void SetCommander(ICommander commander);
    }
}
