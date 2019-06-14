using SauceEditor.Helpers;
using SauceEditor.Models.Components;
using SauceEditor.Views.Factories;
using SpiceEngine.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace SauceEditor.ViewModels.Trees
{
    public class ComponentListViewModel<T> : ViewModel, IComponentListViewModel where T : Component
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public bool IsExpanded { get; set; }
        public ReadOnlyCollection<ComponentViewModel<T>> Children { get; set; }

        public RelayCommand MenuCommand { get; set; }
        public RelayCommand CreateCommand { get; set; }

        public ContextMenu ContextMenu { get; set; }

        public ComponentListViewModel(IEnumerable<T> components, IComponentFactory componentFactory)
        {
            Name = typeof(T).Name + "s";
            CreateCommand = GetCreateCommand(componentFactory);

            ContextMenu = new ContextMenu();
            ContextMenu.Items.Add(new MenuItem()
            {
                Header = "Add " + typeof(T).Name,
                Command = CreateCommand
            });

            Children = new ReadOnlyCollection<ComponentViewModel<T>>(components.Select(c => new ComponentViewModel<T>(c, componentFactory)).ToList());
        }

        private RelayCommand GetCreateCommand(IComponentFactory componentFactory) => new TypeSwitch<RelayCommand>()
            .Case<MapComponent>(() => new RelayCommand(p => componentFactory.CreateMap()))
            .Case<ModelComponent>(() => new RelayCommand(p => componentFactory.CreateModel()))
            .Case<BehaviorComponent>(() => new RelayCommand(p => componentFactory.CreateBehavior()))
            .Case<TextureComponent>(() => new RelayCommand(p => componentFactory.CreateTexture()))
            .Case<SoundComponent>(() => new RelayCommand(p => componentFactory.CreateSound()))
            .Case<MaterialComponent>(() => new RelayCommand(p => componentFactory.CreateMaterial()))
            .Case<ArchetypeComponent>(() => new RelayCommand(p => componentFactory.CreateArchetype()))
            .Case<ScriptComponent>(() => new RelayCommand(p => componentFactory.CreateScript()))
            .Default(() => throw new NotImplementedException())
            .Match<T>();

        /*<ContextMenu x:Key="MapsMenu">
            <MenuItem Header="Add Map" Command="{StaticResource AddMapCommand}"/>
        </ContextMenu>*/
    }
}