namespace SpiceEngineCore.Game
{
    public interface IRender
    {
        bool IsLoaded { get; }
        double Frequency { get; set; }

        void Tick();
    }
}
