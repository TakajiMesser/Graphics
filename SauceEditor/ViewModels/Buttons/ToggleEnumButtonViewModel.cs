using OpenTK;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace SauceEditor.ViewModels.Buttons
{
    public class ToggleEnumButtonViewModel : ViewModel
    {
        public Type EnumType { get; set; }


        public ObservableCollection<ButtonViewModel> Children { get; set; } = new ObservableCollection<ButtonViewModel>();

        public Vector3 Transform { get; set; }
        public NumericUpDownViewModel XViewModel { get; set; }

        public void OnXViewModelChanged() => AddChild(XViewModel, (s, args) => Transform = Vector3.UnitX * XViewModel.Value);

        public ToggleEnumButtonViewModel()
        {
            //ChildPropertyChanged += (s, args) => Transform = new Vector3(XViewModel.Value, YViewModel.Value, ZViewModel.Value);
        }

        public void UpdateFromModel(Vector3 transform)
        {
            Transform = transform;
            XViewModel.Value = transform.X;
        }
    }
}