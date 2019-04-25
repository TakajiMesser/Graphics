using NodeNetwork.ViewModels;
using NodeNetwork.Views;
using OpenTK;
using ReactiveUI;

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