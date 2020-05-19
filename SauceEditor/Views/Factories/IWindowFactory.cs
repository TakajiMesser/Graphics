using SpiceEngineCore.Maps;

namespace SauceEditor.Views.Factories
{
    public interface IWindowFactory
    {
        void CreateGameWindow(IMap map);
        void CreateSettingsWindow();
    }
}