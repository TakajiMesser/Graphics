using SauceEditor.Views.Factories;
using SauceEditorCore.Models.Entities;
using System.Windows.Controls;

namespace SauceEditor.ViewModels.Trees.Entities
{
    public class EntityViewModel : ViewModel
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public bool IsExpanded { get; set; }

        public RelayCommand SelectCommand { get; set; }
        public RelayCommand DuplicateCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }

        public ContextMenu ContextMenu { get; set; }

        private MapEntityID _mapEntityID;

        public EntityViewModel(MapEntityID mapEntityID, IEntityFactory entityFactory)
        {
            _mapEntityID = mapEntityID;
            Name = _mapEntityID.Name;

            SelectCommand = new RelayCommand(p => entityFactory.SelectEntity(_mapEntityID.ID));
            DuplicateCommand = new RelayCommand(p => entityFactory.DuplicateEntity(_mapEntityID.ID));
            DeleteCommand = new RelayCommand(p => entityFactory.DeleteEntity(_mapEntityID.ID));

            ContextMenu = new ContextMenu();
            ContextMenu.Items.Add(new MenuItem()
            {
                Header = "Select",
                Command = SelectCommand
            });
            ContextMenu.Items.Add(new MenuItem()
            {
                Header = "Duplicate",
                Command = DuplicateCommand
            });
            ContextMenu.Items.Add(new MenuItem()
            {
                Header = "Delete",
                Command = DeleteCommand
            });
        }
    }
}