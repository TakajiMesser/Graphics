using SauceEditorCore.Models.Libraries;
using System.Windows.Media.Imaging;

namespace SauceEditor.ViewModels.Libraries
{
    public abstract class PathInfoViewModel : ViewModel
    {
        public IPathInfo PathInfo { get; set; }

        public BitmapImage PreviewIcon { get; set; }
        public RelayCommand OpenCommand { get; set; }
        
        public abstract void LoadPreviewIcon();

        public void Refresh() => PathInfo.Refresh();
    }
}