using SauceEditor.Views.Factories;
using SauceEditorCore.Models.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace SauceEditor.ViewModels.Trees.Entities
{
    public class EntityTypeViewModel : ViewModel
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public bool IsExpanded { get; set; }
        public ReadOnlyCollection<EntityViewModel> Children { get; set; }

        public RelayCommand MenuCommand { get; set; }
        public RelayCommand CreateCommand { get; set; }

        public ContextMenu ContextMenu { get; set; }

        public EntityTypeViewModel(string name, IEnumerable<MapEntityID> mapEntityIDs, IEntityFactory entityFactory)
        {
            Name = name;
            //CreateCommand = GetCreateCommand(entityFactory);

            ContextMenu = new ContextMenu();
            ContextMenu.Items.Add(new MenuItem()
            {
                Header = "Add " + Name,
                Command = CreateCommand
            });

            Children = new ReadOnlyCollection<EntityViewModel>(mapEntityIDs.Select(e => new EntityViewModel(e, entityFactory)).ToList());
        }

        /*private RelayCommand GetCreateCommand(IEntityFactory entityFactory) => new TypeSwitch<RelayCommand>()
            .Case<ILight>(() => new RelayCommand(p => entityFactory.CreateLight()))
            .Case<Brush>(() => new RelayCommand(p => entityFactory.CreateBrush()))
            .Case<IActor>(() => new RelayCommand(p => entityFactory.CreateActor()))
            .Case<Volume>(() => new RelayCommand(p => entityFactory.CreateVolume()))
            .Default(() => throw new NotImplementedException())
            .Match<T>();*/

        /*<ContextMenu x:Key="MapsMenu">
            <MenuItem Header="Add Map" Command="{StaticResource AddMapCommand}"/>
        </ContextMenu>*/
    }
}