using SauceEditor.Models.Components;
using SauceEditor.Views.Factories;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SauceEditor.ViewModels.Trees
{
    public class ComponentListViewModel : ViewModel
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public bool IsExpanded { get; set; }
        public ReadOnlyCollection<ComponentViewModel> Children { get; set; }

        public ComponentListViewModel(string name, IEnumerable<Component> components, IComponentFactory componentFactory)
        {
            Name = name;
            Children = new ReadOnlyCollection<ComponentViewModel>(components.Select(c => new ComponentViewModel(c, componentFactory)).ToList());
        }
    }
}