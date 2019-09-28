using SauceEditor.Models.Components;
using SauceEditor.ViewModels.Docks;
using SauceEditor.Views.Factories;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SauceEditor.ViewModels.Trees
{
    public class ProjectTreePanelViewModel : DockViewModel
    {
        private List<ProjectViewModel> _roots = new List<ProjectViewModel>();

        public ProjectTreePanelViewModel() : base(DockTypes.Property) => Roots = new ReadOnlyCollection<ProjectViewModel>(_roots);

        public ReadOnlyCollection<ProjectViewModel> Roots { get; set; }

        public void UpdateFromModel(Project project, IComponentFactory componentFactory)
        {
            _roots.Add(new ProjectViewModel(project, componentFactory));
            Roots = new ReadOnlyCollection<ProjectViewModel>(_roots);
        }
    }
}