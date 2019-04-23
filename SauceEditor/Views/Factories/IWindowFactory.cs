using SauceEditor.Models.Components;

namespace SauceEditor.Views.Factories
{
    public interface IWindowFactory
    {
        void CreateGameWindow(Map map);
        void CreateSettingsWindow();
    }
}