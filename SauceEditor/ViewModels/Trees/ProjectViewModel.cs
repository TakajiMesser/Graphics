using SauceEditor.Models.Components;
using SauceEditor.Views.Factories;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SauceEditor.ViewModels.Trees
{
    public class ProjectViewModel : ViewModel
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public bool IsExpanded { get; set; }
        public ReadOnlyCollection<ComponentListViewModel> Children { get; set; }

        public ProjectViewModel(Project project, IComponentFactory componentFactory)
        {
            Name = project.Name;

            Children = new ReadOnlyCollection<ComponentListViewModel>(new List<ComponentListViewModel>()
            {
                new ComponentListViewModel("Maps", project.Maps, componentFactory),
                new ComponentListViewModel("Models", project.Models, componentFactory),
                new ComponentListViewModel("Behaviors", project.Behaviors, componentFactory),
                new ComponentListViewModel("Textures", project.Textures, componentFactory),
                new ComponentListViewModel("Sounds", project.Sounds, componentFactory),
                new ComponentListViewModel("Materials", project.Materials, componentFactory),
                new ComponentListViewModel("Archetypes", project.Archetypes, componentFactory),
                new ComponentListViewModel("Scripts", project.Scripts, componentFactory)
            });
        }
    }
}