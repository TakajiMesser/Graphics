using NodeNetwork.ViewModels;
using SauceEditor.ViewModels;
using SauceEditor.ViewModels.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SauceEditor.Views.Behaviors
{
    /// <summary>
    /// Interaction logic for BehaviorView.xaml
    /// </summary>
    public partial class BehaviorView : DockPanel
    {
        public BehaviorView()
        {
            InitializeComponent();

            //Network.Background = 

            //Network.ViewModel = new BehaviorNetworkViewModel();

            //Create a new viewmodel for the NetworkView
            var network = new NetworkViewModel();

            //Create the node for the first node, set its name and add it to the network.
            var node1 = new NodeViewModel
            {
                Name = "Node 1"
            };
            network.Nodes.Add(node1);

            //Create the viewmodel for the input on the first node, set its name and add it to the node.
            var node1Input = new NodeInputViewModel
            {
                Name = "Node 1 input"
            };
            node1.Inputs.Add(node1Input);

            //Create the second node viewmodel, set its name, add it to the network and add an output in a similar fashion.
            var node2 = new NodeViewModel
            {
                Name = "Node 2"
            };
            network.Nodes.Add(node2);

            var node2Output = new NodeOutputViewModel
            {
                Name = "Node 2 output"
            };
            node2.Outputs.Add(node2Output);

            //Assign the viewmodel to the view.
            Network.ViewModel = network;
            Network.UpdateLayout();
        }
    }
}
