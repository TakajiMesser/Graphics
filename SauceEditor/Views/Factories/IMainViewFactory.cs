using SauceEditor.Models.Components;
using SauceEditor.ViewModels;

namespace SauceEditor.Views.Factories
{
    public interface IMainViewFactory
    {
        ViewModel CreateGamePanelView(Map map);

        void SaveAll();
        void LoadSettings();
    }
}