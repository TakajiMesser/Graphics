namespace SpiceEngine.Game
{
    public interface IUpdate
    {
        void Update();
        event EventHandler<EventArgs> Updated;
    }
}
