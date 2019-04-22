using SauceEditor.ViewModels;
using SauceEditor.Models.Components;

namespace SauceEditor.Views.Factories
{
    public interface IMainViewFactory
    {
        ViewModel CreateGamePanelView(Map map);
        void OpenProject(string filePath);
        void OpenMap(string filePath);
        void OpenModel(string filePath);
    }
}