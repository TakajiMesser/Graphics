using SauceEditor.Views.Custom;
using SpiceEngine.Rendering.Meshes;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SauceEditor.ViewModels.Tools.Primitives
{
    public abstract class Primitive : IImageButton
    {
        public Primitive(string name) => Name = name;

        public string Name { get; }
        public BitmapSource Icon => null;

        public abstract ModelMesh MeshShape { get; }

        private RelayCommand _openCommand;
        public RelayCommand OpenCommand => (_openCommand = _openCommand ?? new RelayCommand(
            p => { },
            p => true
        ));

        private RelayCommand _dragCommand;
        public RelayCommand DragCommand => (_dragCommand = _dragCommand ?? new RelayCommand(
            p =>
            {
                var draggedItem = p as DependencyObject;

                var data = new DataObject();
                data.SetData(typeof(ModelMesh), MeshShape);

                DragDrop.DoDragDrop(draggedItem, data, DragDropEffects.Move);
            },
            p => true
        ));
    }
}
