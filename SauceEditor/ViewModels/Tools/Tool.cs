using SauceEditor.Views.Custom;
using SpiceEngineCore.Maps;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SauceEditor.ViewModels.Tools
{
    public abstract class Tool<T> : IImageButton where T : IMapEntity
    {
        private RelayCommand _dragCommand;

        public Tool(string name) => Name = name;

        [Browsable(false)]
        public string Name { get; }

        [Browsable(false)]
        public BitmapSource Icon => null;

        [Browsable(false)]
        public abstract T MapEntity { get; }

        [Browsable(false)]
        public abstract RelayCommand OpenCommand { get; }

        [Browsable(false)]
        public RelayCommand DragCommand => _dragCommand ?? (_dragCommand = new RelayCommand(
            p =>
            {
                var draggedItem = p as DependencyObject;

                var data = new DataObject();
                data.SetData(typeof(T), MapEntity);

                DragDrop.DoDragDrop(draggedItem, data, DragDropEffects.Move);
            },
            p => true
        ));
    }
}
