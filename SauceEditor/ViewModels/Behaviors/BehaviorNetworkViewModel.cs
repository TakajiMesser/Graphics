using NodeNetwork.ViewModels;
using OpenTK;

namespace SauceEditor.ViewModels.Behaviors
{
    public class BehaviorNetworkViewModel : NetworkViewModel
    {
        private Vector3 _transform;

        public Vector3 Transform
        {
            get => _transform;
            set => _transform = value;//SetProperty(ref _transform, value);
        }

        public BehaviorNetworkViewModel()
        {
            Nodes.Add(new BehaviorNodeViewModel()
            {
                Name = "Node Uno"
            });
        }
    }
}