using SauceEditor.Helpers;
using SauceEditor.ViewModels.Trees.Entities.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock.Layout;

namespace SauceEditor.Views.Trees.Entities
{
    /// <summary>
    /// Interaction logic for EntityTreePanel.xaml
    /// </summary>
    public partial class EntityTreePanel : LayoutAnchorable, IRearrange
    {
        public EntityTreePanel()
        {
            InitializeComponent();
            ViewModel.Rearranger = this;
        }

        public void Rearrange(string name, DragEventArgs args)
        {
            var layerRoot = LayerTree.ItemContainerGenerator.ContainerFromIndex(0) as TreeViewItem;
            var dragIndex = GetIndexOf(layerRoot.Items, l => l.Name == name);

            var items = GetUIElements(layerRoot);
            var dropIndex = DragHelper.GetDropIndex(items.ToList(), args);

            // TODO - Lock appropriately, since multiple drag and drops can break this if done too quickly
            // Only update the view model if the index has changed
            if (dragIndex != dropIndex)
            {
                ViewModel.LayerRoots.First().MoveChild(name, dropIndex);
            }
        }

        private int GetIndexOf(ItemCollection items, Predicate<LayerViewModel> predicate)
        {
            for (var i = 0; i < items.Count; i++)
            {
                var item = items.GetItemAt(i) as LayerViewModel;
                if (predicate(item))
                {
                    return i;
                }
            }

            return -1;
        }

        private IEnumerable<UIElement> GetUIElements(ItemsControl itemsControl)
        {
            for (var i = 0; i < itemsControl.Items.Count; i++)
            {
                yield return itemsControl.ItemContainerGenerator.ContainerFromIndex(i) as UIElement;
            }
        }
    }
}
