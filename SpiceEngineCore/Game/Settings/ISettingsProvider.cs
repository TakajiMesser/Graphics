namespace SpiceEngineCore.Game.Settings
{
    public interface ISettingsProvider
    {
        T GetSubSettings<T>() where T : ISubSettings;
        T GetSubSettingsOrDefault<T>() where T : ISubSettings;

        bool HasSubSettings<T>() where T : ISubSettings;
    }
}
