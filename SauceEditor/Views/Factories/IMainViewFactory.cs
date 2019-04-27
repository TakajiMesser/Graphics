using SauceEditor.Models.Components;
using SauceEditor.ViewModels;

namespace SauceEditor.Views.Factories
{
    public interface IMainViewFactory
    {
        ViewModel CreateGamePanelManager(Map map);
        ViewModel CreateBehaviorView(Behavior behavior);
        ViewModel CreateScriptView(Script script);

        void SaveAll();
        void LoadSettings();
    }
}