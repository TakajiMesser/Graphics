using OpenTK;

namespace SauceEditor.ViewModels
{
    public class TransformPanelViewModel : ViewModel
    {
        public Vector3 Transform { get; set; }

        public NumericUpDownViewModel XViewModel { get; set; }
        public NumericUpDownViewModel YViewModel { get; set; }
        public NumericUpDownViewModel ZViewModel { get; set; }

        public void OnXViewModelChanged() => AddChild(XViewModel, (s, args) => Transform = new Vector3(XViewModel.Value, YViewModel.Value, ZViewModel.Value));//ChildViewModels.Add(XViewModel);
        public void OnYViewModelChanged() => AddChild(YViewModel, (s, args) => Transform = new Vector3(XViewModel.Value, YViewModel.Value, ZViewModel.Value));//ChildViewModels.Add(YViewModel);
        public void OnZViewModelChanged() => AddChild(ZViewModel, (s, args) => Transform = new Vector3(XViewModel.Value, YViewModel.Value, ZViewModel.Value));// ChildViewModels.Add(ZViewModel);

        //public void OnPositionViewModelChanged() => AddChild(PositionViewModel, (s, args) => Position = PositionViewModel.Transform);
        //public void OnRotationViewModelChanged() => AddChild(RotationViewModel, (s, args) => Rotation = RotationViewModel.Transform);
        //public void OnScaleViewModelChanged() => AddChild(ScaleViewModel, (s, args) => Scale = ScaleViewModel.Transform);

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