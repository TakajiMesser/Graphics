using SauceEditor.Views.Factories;
using SauceEditorCore.Models.Components;
using SpiceEngineCore.Helpers;
using System;
using System.Windows.Controls;

namespace SauceEditor.ViewModels.Trees
{
    public class ComponentViewModel<T> : ViewModel where T : Component
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public bool IsExpanded { get; set; }

        public RelayCommand OpenCommand { get; set; }
        public RelayCommand MenuCommand { get; set; }
        public RelayCommand ExcludeCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand RenameCommand { get; set; }

        public ContextMenu ContextMenu { get; set; }

        T _component;

        public ComponentViewModel(T component, IComponentFactory componentFactory)
        {
            _component = component;
            Name = _component.Name;
            OpenCommand = GetOpenCommand(componentFactory);

            ContextMenu = new ContextMenu();
            ContextMenu.Items.Add(new MenuItem()
            {
                Header = "Open " + typeof(T).Name,
                Command = OpenCommand
            });
            ContextMenu.Items.Add(new MenuItem()
            {
                Header = "Exclude from Project",
                Command = ExcludeCommand
            });
            ContextMenu.Items.Add(new MenuItem()
            {
                Header = "Delete",
                Command = DeleteCommand
            });
            ContextMenu.Items.Add(new MenuItem()
            {
                Header = "Rename",
                Command = RenameCommand
            });
        }

        private RelayCommand GetOpenCommand(IComponentFactory componentFactory) => new TypeSwitch<RelayCommand>()
            .Case<MapComponent>(() => new RelayCommand(p => componentFactory.OpenMap(_component.Path)))
            .Case<ModelComponent>(() => new RelayCommand(p => componentFactory.OpenModel(_component.Path)))
            .Case<BehaviorComponent>(() => new RelayCommand(p => componentFactory.OpenBehavior(_component.Path)))
            .Case<TextureComponent>(() => new RelayCommand(p => componentFactory.OpenTexture(_component.Path)))
            .Case<SoundComponent>(() => new RelayCommand(p => componentFactory.OpenSound(_component.Path)))
            .Case<MaterialComponent>(() => new RelayCommand(p => componentFactory.OpenMaterial(_component.Path)))
            .Case<ArchetypeComponent>(() => new RelayCommand(p => componentFactory.OpenArchetype(_component.Path)))
            .Case<ScriptComponent>(() => new RelayCommand(p => componentFactory.OpenScript(_component.Path)))
            .Default(() => throw new NotImplementedException())
            .Match<T>();

        /*var item = s as TreeViewItem;
        item.IsSelected = true;
        item.ContextMenu = Tree.FindResource("MapMenu") as ContextMenu;

        <ContextMenu x:Key="MapMenu">
            <MenuItem Header="Open Map" Command="{StaticResource OpenMapCommand}"/>
            <Separator/>
            <MenuItem Header="Exclude From Project" Command="{StaticResource ExcludeCommand}"/>
            <MenuItem Header="Delete" Command="{StaticResource DeleteCommand}"/>
            <MenuItem Header="Rename" Command="{StaticResource RenameCommand}"/>
        </ContextMenu>*/
    }
}