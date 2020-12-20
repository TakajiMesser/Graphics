using NodeNetwork.ViewModels;
using NodeNetwork.Views;
using ReactiveUI;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SauceEditor.ViewModels.Behaviors
{
    public class BehaviorNodeViewModel : NodeViewModel
    {
        private Vector3 _transform;

        public Vector3 Transform
        {
            get => _transform;
            set => _transform = value;//SetProperty(ref _transform, value);
        }

        static BehaviorNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new NodeView(), typeof(IViewFor<BehaviorNodeViewModel>));
        }

        //public ShaderNodeOutputViewModel ColorOutput { get; } = new ShaderNodeOutputViewModel();

        public BehaviorNodeViewModel()
        {
            this.Name = "Color";

            var inputNode = new NodeInputViewModel
            {
                Name = "Node 1 input"
                /*ColorOutput.Editor = editor;
                ColorOutput.ReturnType = typeof(Vec3);
                ColorOutput.Value = editor.ValueChanged;*/
            };
            Inputs.Add(inputNode);

            /*ColorEditorViewModel editor = new ColorEditorViewModel();
            ColorOutput.Name = "Color";
            ColorOutput.Editor = editor;
            ColorOutput.ReturnType = typeof(Vec3);
            ColorOutput.Value = editor.ValueChanged;
            this.Outputs.Add(ColorOutput);*/
        }
    }
}