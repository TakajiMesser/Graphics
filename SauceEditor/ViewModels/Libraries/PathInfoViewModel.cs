using SauceEditor.Views.Custom;
using SauceEditorCore.Models.Libraries;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace SauceEditor.ViewModels.Libraries
{
    public abstract class PathInfoViewModel : ViewModel, IImageButton
    {
        private RelayCommand _selectCommand;

        public string Name => PathInfo.Name;
        public BitmapSource Icon => PreviewIcon;

        public IPathInfo PathInfo { get; set; }

        public BitmapImage PreviewIcon { get; set; }
        //public RelayCommand SelectCommand { get; set; }
        public RelayCommand OpenCommand { get; set; }
        public RelayCommand DragCommand { get; set; }

        public RelayCommand SelectCommand => _selectCommand ?? (_selectCommand = new RelayCommand(
            p => { },
            p => true
        ));

        public abstract void LoadPreviewIcon();

        public void Refresh() => PathInfo.Refresh();
    }
}