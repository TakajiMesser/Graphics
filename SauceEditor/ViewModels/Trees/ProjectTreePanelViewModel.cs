using SauceEditor.Models.Components;
using SauceEditor.Views.Factories;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SauceEditor.ViewModels.Trees
{
    public class ProjectTreePanelViewModel : ViewModel
    {
        public ReadOnlyCollection<ProjectViewModel> Roots { get; set; }

        private List<ProjectViewModel> _roots = new List<ProjectViewModel>();

        public ProjectTreePanelViewModel()
        {
            Roots = new ReadOnlyCollection<ProjectViewModel>(_roots);
        }

        public void UpdateFromModel(Project project, IComponentFactory componentFactory)
        {
            _roots.Add(new ProjectViewModel(project, componentFactory));
            Roots = new ReadOnlyCollection<ProjectViewModel>(_roots);
        }
    }
}