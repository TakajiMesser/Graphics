using NodeNetwork.ViewModels;

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