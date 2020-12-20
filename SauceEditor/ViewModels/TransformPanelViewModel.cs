using PropertyChanged;
using SauceEditor.Views.Custom;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

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