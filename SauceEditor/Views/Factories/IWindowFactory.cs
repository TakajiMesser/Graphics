using SpiceEngine.Maps;

namespace SauceEditor.Views.Factories
{
    public interface IWindowFactory
    {
        void CreateGameWindow(Map map);
        void CreateSettingsWindow();
    }
}