using SauceEditor.Helpers;
using SauceEditor.Models.Components;
using SauceEditor.Views.Factories;
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
            .Case<Map>(() => new RelayCommand(p => componentFactory.CreateMap()))
            .Case<Model>(() => new RelayCommand(p => componentFactory.CreateModel()))
            .Case<Behavior>(() => new RelayCommand(p => componentFactory.CreateBehavior()))
            .Case<Texture>(() => new RelayCommand(p => componentFactory.CreateTexture()))
            .Case<Sound>(() => new RelayCommand(p => componentFactory.CreateSound()))
            .Case<Material>(() => new RelayCommand(p => componentFactory.CreateMaterial()))
            .Case<Archetype>(() => new RelayCommand(p => componentFactory.CreateArchetype()))
            .Case<Script>(() => new RelayCommand(p => componentFactory.CreateScript()))
            .Default(() => throw new NotImplementedException())
            .Match<T>();

        /*<ContextMenu x:Key="MapsMenu">
            <MenuItem Header="Add Map" Command="{StaticResource AddMapCommand}"/>
        </ContextMenu>*/
    }
}