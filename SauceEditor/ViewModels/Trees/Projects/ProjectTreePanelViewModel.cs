using SauceEditor.Models.Components;
using SauceEditor.ViewModels.Docks;
using SauceEditor.Views.Factories;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SauceEditor.ViewModels.Trees.Projects
{
    public class ProjectTreePanelViewModel : DockableViewModel
    {
        private List<ProjectViewModel> _roots = new List<ProjectViewModel>();

        public ProjectTreePanelViewModel() => Roots = new ReadOnlyCollection<ProjectViewModel>(_roots);

        public ReadOnlyCollection<ProjectViewModel> Roots { get; set; }

        public void UpdateFromModel(Project project, IComponentFactory componentFactory)
        {
            _roots.Add(new ProjectViewModel(project, componentFactory));
            Roots = new ReadOnlyCollection<ProjectViewModel>(_roots);
            IsActive = true;
        }
    }
}