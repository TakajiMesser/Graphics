using SauceEditor.Models.Components;
using SauceEditor.Views.Factories;
using SauceEditorCore.Models.Components;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SauceEditor.ViewModels.Trees.Projects
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
                new ComponentListViewModel<MapComponent>(project.MapComponents, componentFactory),
                new ComponentListViewModel<ModelComponent>(project.ModelComponents, componentFactory),
                new ComponentListViewModel<BehaviorComponent>(project.BehaviorComponents, componentFactory),
                new ComponentListViewModel<TextureComponent>(project.TextureComponents, componentFactory),
                new ComponentListViewModel<SoundComponent>(project.SoundComponents, componentFactory),
                new ComponentListViewModel<MaterialComponent>(project.MaterialComponents, componentFactory),
                new ComponentListViewModel<ArchetypeComponent>(project.ArchetypeComponents, componentFactory),
                new ComponentListViewModel<ScriptComponent>(project.ScriptComponents, componentFactory)
            });
        }
    }
}