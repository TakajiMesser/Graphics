using SauceEditor.ViewModels;
using SauceEditorCore.Models.Components;

namespace SauceEditor.Views.Factories
{
    public interface IGameDockFactory
    {
        ViewModel CreateGamePanel(MapComponent mapComponent, Component component = null);
        ViewModel CreateBehaviorView(BehaviorComponent behaviorComponent);
        ViewModel CreateScriptView(ScriptComponent scriptComponent);
    }
}