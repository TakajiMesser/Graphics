namespace SauceEditor.ViewModels.Trees.Projects
{
    public interface IComponentListViewModel
    {
        string Name { get; set; }
        bool IsSelected { get; set; }
        bool IsExpanded { get; set; }
    }
}