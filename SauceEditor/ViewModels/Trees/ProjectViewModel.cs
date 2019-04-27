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
        public ReadOnlyCollection<IComponentListViewModel> Children { get; set; }

        public ProjectViewModel(Project project, IComponentFactory componentFactory)
        {
            Name = project.Name;

            Children = new ReadOnlyCollection<IComponentListViewModel>(new List<IComponentListViewModel>()
            {
                new ComponentListViewModel<Map>(project.Maps, componentFactory),
                new ComponentListViewModel<Model>(project.Models, componentFactory),
                new ComponentListViewModel<Behavior>(project.Behaviors, componentFactory),
                new ComponentListViewModel<Texture>(project.Textures, componentFactory),
                new ComponentListViewModel<Sound>(project.Sounds, componentFactory),
                new ComponentListViewModel<Material>(project.Materials, componentFactory),
                new ComponentListViewModel<Archetype>(project.Archetypes, componentFactory),
                new ComponentListViewModel<Script>(project.Scripts, componentFactory)
            });
        }
    }
}