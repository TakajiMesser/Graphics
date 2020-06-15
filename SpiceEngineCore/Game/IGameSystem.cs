namespace SpiceEngineCore.Game
{
    public interface IGameSystem : ITick, IUpdate
    {
        int TickRate { get; set; }
    }
}
