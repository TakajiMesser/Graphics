using OpenTK;
using System.Windows.Controls;

namespace SauceEditor.ViewModels.Buttons
{
    public class ButtonViewModel : ViewModel
    {
        public string Name { get; set; }
        public RelayCommand Command { get; set; }

        public ButtonViewModel(string name)
        {
            Name = name;
        }
    }
}