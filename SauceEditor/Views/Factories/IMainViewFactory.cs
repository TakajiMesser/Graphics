using SauceEditor.Models.Components;
using SauceEditor.ViewModels;
using Map = SpiceEngine.Maps.Map;

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