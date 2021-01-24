namespace SpiceEngineCore.Game.Settings
{
    public enum Quality
    {
        VeryLow,
        Low,
        Medium,
        High,
        VeryHigh
    }

    public interface ISubSettings
    {
        string Title { get; }
    }
}
