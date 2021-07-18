namespace SpiceEngineCore.Game.Settings
{
    public class SubSettings : ISubSettings
    {
        public SubSettings(string title) => Title = title;

        public string Title { get; }
    }
}
