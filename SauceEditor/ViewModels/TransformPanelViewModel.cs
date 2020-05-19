using OpenTK;
using PropertyChanged;
using SauceEditor.Views.Custom;

namespace SauceEditor.ViewModels
{
    public class TransformPanelViewModel : ViewModel
    {
        public Vector3 Transform { get; set; }

        [PropagateChanges]
        [DoNotCheckEquality]
        public NumericUpDownViewModel XViewModel { get; set; }

        [PropagateChanges]
        [DoNotCheckEquality]
        public NumericUpDownViewModel YViewModel { get; set; }

        [PropagateChanges]
        [DoNotCheckEquality]
        public NumericUpDownViewModel ZViewModel { get; set; }

        /*public void OnXViewModelChanged() => AddChild(XViewModel, (s, args) => Transform = new Vector3(XViewModel.Value, YViewModel.Value, ZViewModel.Value));//ChildViewModels.Add(XViewModel);
        public void OnYViewModelChanged() => AddChild(YViewModel, (s, args) => Transform = new Vector3(XViewModel.Value, YViewModel.Value, ZViewModel.Value));//ChildViewModels.Add(YViewModel);
        public void OnZViewModelChanged() => AddChild(ZViewModel, (s, args) => Transform = new Vector3(XViewModel.Value, YViewModel.Value, ZViewModel.Value));// ChildViewModels.Add(ZViewModel);

        private void UpdateTransform() => Transform = new Vector3(XViewModel.Value, YViewModel.Value, ZViewModel.Value);*/

        public TransformPanelViewModel()
        {
            //ChildPropertyChanged += (s, args) => Transform = new Vector3(XViewModel.Value, YViewModel.Value, ZViewModel.Value);
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