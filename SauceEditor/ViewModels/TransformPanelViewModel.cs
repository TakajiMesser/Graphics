using OpenTK;
using System.ComponentModel;

namespace SauceEditor.ViewModels
{
    public class TransformPanelViewModel : ViewModel
    {
        public Vector3 Transform { get; set; }

        public NumericUpDownViewModel XViewModel { get; set; }
        public NumericUpDownViewModel YViewModel { get; set; }
        public NumericUpDownViewModel ZViewModel { get; set; }

        public void OnXViewModelChanged() => ChildViewModels.Add(XViewModel);
        public void OnYViewModelChanged() => ChildViewModels.Add(YViewModel);
        public void OnZViewModelChanged() => ChildViewModels.Add(ZViewModel);

        public TransformPanelViewModel()
        {
            ChildPropertyChanged += (s, args) => Transform = new Vector3(XViewModel.Value, YViewModel.Value, ZViewModel.Value);
        }

        public void UpdateFromModel(Vector3 transform)
        {
            Transform = transform;

            XViewModel.Value = transform.X;
            YViewModel.Value = transform.Y;
            ZViewModel.Value = transform.Z;
        }
    }
}