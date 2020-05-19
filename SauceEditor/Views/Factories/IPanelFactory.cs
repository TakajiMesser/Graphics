using SauceEditorCore.Models.Components;

namespace SauceEditor.Views.Factories
{
    public interface IPanelFactory
    {
        void OpenModelToolPanel();
        void OpenCameraToolPanel();
        void OpenBrushToolPanel();
        void OpenActorToolPanel();
        void OpenLightToolPanel();
        void OpenVolumeToolPanel();

        void CreateGamePanel(MapComponent mapComponent, Component component = null);
        void CreateBehaviorPanel(BehaviorComponent behaviorComponent);
        void CreateScriptPanel(ScriptComponent scriptComponent);

        void OpenProjectTreePanel();
        void OpenLibraryPanel();
        void OpenPropertyPanel();
        void OpenEntityTreePanel();
    }
}
