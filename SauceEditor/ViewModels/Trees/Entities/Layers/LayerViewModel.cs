using SauceEditor.Helpers;
using SauceEditor.Views.Trees.Entities;
using SpiceEngineCore.Entities.Layers;
using System.Windows;
using System.Windows.Controls;

namespace SauceEditor.ViewModels.Trees.Entities.Layers
{
    public class LayerViewModel : ViewModel
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public bool IsExpanded { get; set; }

        public RelayCommand SelectCommand { get; set; }
        public RelayCommand DuplicateCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }

        private RelayCommand _dragCommand;
        public RelayCommand DragCommand
        {
            get => _dragCommand ?? (_dragCommand = new RelayCommand(
                p =>
                {
                    var draggedItem = ViewHelper.FindAncestor<TreeViewItem>((DependencyObject)p);
                    //var dataItem = draggedItem.ItemContainerGenerator.ItemFromContainer(null);

                    DragDrop.DoDragDrop(draggedItem, Name, DragDropEffects.Move);
                },
                p => true
            ));
        }

        private RelayCommand _dropCommand;
        public RelayCommand DropCommand
        {
            get => _dropCommand ?? (_dropCommand = new RelayCommand(
                p =>
                {
                    var args = (DragEventArgs)p;

                    var name = args.Data.GetData(DataFormats.StringFormat);
                    /*var originalSource = args.OriginalSource;
                    var source = args.Source;
                    var position = args.GetPosition(null);*/
                    _rearranger.Rearrange((string)name, args);
                },
                p => true
            ));
        }

        public ContextMenu ContextMenu { get; set; }

        private IRearrange _rearranger;

        public LayerViewModel(string layerName, LayerTypes layerType, ILayerProvider layerProvider, IRearrange rearranger)
        {
            Name = layerName;
            _rearranger = rearranger;

            //SelectCommand = new RelayCommand(p => entityFactory.SelectEntity(_mapEntityID.ID));
            //DuplicateCommand = new RelayCommand(p => entityFactory.DuplicateEntity(_mapEntityID.ID));
            //DeleteCommand = new RelayCommand(p => entityFactory.DeleteEntity(_mapEntityID.ID));

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