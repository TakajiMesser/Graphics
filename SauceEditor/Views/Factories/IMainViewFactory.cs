using SauceEditor.Models.Components;
using SauceEditor.ViewModels;

namespace SauceEditor.Views.Factories
{
    public interface IMainViewFactory
    {
        ViewModel CreateGamePanelManager(MapComponent mapComponent);
        ViewModel CreateBehaviorView(BehaviorComponent behaviorComponent);
        ViewModel CreateScriptView(ScriptComponent scriptComponent);

        void SetActiveInMainDock(IMainDockViewModel viewModel);

        void SaveAll();
        void LoadSettings();
    }
}