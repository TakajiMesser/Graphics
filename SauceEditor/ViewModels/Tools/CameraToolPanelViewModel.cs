using PropertyChanged;
using SauceEditor.ViewModels.Docks;
using SauceEditor.ViewModels.Tools.Cameras;
using SauceEditor.Views.Custom;
using SauceEditor.Views.Factories;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SauceEditor.ViewModels.Tools
{
    public class CameraToolPanelViewModel : DockableViewModel
    {
        private List<CameraTool> _children = new List<CameraTool>();

        [PropagateChanges]
        [DoNotCheckEquality]
        public CameraTool Tool { get; set; }

        public object Selection { get; set; }

        public CameraToolPanelViewModel()
        {
            _children.Add(new OrthographicCameraTool());
            _children.Add(new PerspectiveCameraTool());

            Children = new ObservableCollection<CameraTool>(_children);
        }

        public IEntityFactory EntityFactory { get; set; }

        public ObservableCollection<CameraTool> Children { get; set; }
    }
}