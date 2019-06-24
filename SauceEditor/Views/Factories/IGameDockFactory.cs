using SauceEditor.Models.Components;
using SauceEditor.ViewModels;
using SauceEditor.ViewModels.Docks;
using SauceEditorCore.Models.Components;

namespace SauceEditor.Views.Factories
{
    public interface IGameDockFactory
    {
        ViewModel CreateGamePanelManager(MapComponent mapComponent, Component component = null);
        ViewModel CreateBehaviorView(BehaviorComponent behaviorComponent);
        ViewModel CreateScriptView(ScriptComponent scriptComponent);
    }
}