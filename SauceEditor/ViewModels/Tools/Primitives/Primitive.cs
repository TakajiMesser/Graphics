using SauceEditor.Views.Custom;
using System.Windows.Media.Imaging;

namespace SauceEditor.ViewModels.Tools.Primitives
{
    public class Primitive : IImageButton
    {
        public Primitive(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public BitmapSource Icon => null;

        private RelayCommand _openCommand;
        public RelayCommand OpenCommand => (_openCommand = _openCommand ?? new RelayCommand(
            p => { },
            p => true
        ));
    }
}
