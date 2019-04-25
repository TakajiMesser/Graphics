using SauceEditor.Models.Components;
using SauceEditor.Views.Factories;

namespace SauceEditor.ViewModels.Trees
{
    public class ComponentViewModel : ViewModel
    {
        public string Name => _component.Name;
        public bool IsSelected { get; set; }
        public bool IsExpanded { get; set; }

        public RelayCommand OpenCommand { get; set; }

        Component _component;

        public ComponentViewModel(Component component, IComponentFactory componentFactory)
        {
            _component = component;

            switch (component)
            {
                case Map map:
                    OpenCommand = new RelayCommand(p => componentFactory.OpenMap(_component.Path));
                    break;
                case Model model:
                    OpenCommand = new RelayCommand(p => componentFactory.OpenModel(_component.Path));
                    break;
                case Behavior behavior:
                    OpenCommand = new RelayCommand(p => componentFactory.OpenBehavior(_component.Path));
                    break;
                case Texture texture:
                    OpenCommand = new RelayCommand(p => componentFactory.OpenTexture(_component.Path));
                    break;
                case Sound sound:
                    OpenCommand = new RelayCommand(p => componentFactory.OpenSound(_component.Path));
                    break;
                case Material material:
                    OpenCommand = new RelayCommand(p => componentFactory.OpenMaterial(_component.Path));
                    break;
                case Archetype archetype:
                    OpenCommand = new RelayCommand(p => componentFactory.OpenArchetype(_component.Path));
                    break;
                case Script script:
                    OpenCommand = new RelayCommand(p => componentFactory.OpenScript(_component.Path));
                    break;
            }
            
        }
    }
}