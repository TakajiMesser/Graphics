using SauceEditor.Models.Components;
using SauceEditor.ViewModels;

namespace SauceEditor.Views.Factories
{
    public interface IMainViewFactory
    {
        ViewModel CreateGamePanelManager(MapComponent mapComponent, Component component = null);
        ViewModel CreateBehaviorView(BehaviorComponent behaviorComponent);
        ViewModel CreateScriptView(ScriptComponent scriptComponent);

        void SetActiveInMainDock(MainDockViewModel viewModel);
    }
}