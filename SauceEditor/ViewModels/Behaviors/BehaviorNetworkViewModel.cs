using NodeNetwork.ViewModels;
using OpenTK;
using SauceEditor.Models;
using SauceEditor.Utilities;
using SpiceEngine.Entities;
using SpiceEngine.Maps;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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