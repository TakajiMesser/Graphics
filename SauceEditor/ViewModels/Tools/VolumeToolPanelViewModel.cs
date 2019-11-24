using PropertyChanged;
using SauceEditor.ViewModels.Docks;
using SauceEditor.ViewModels.Tools.Volumes;
using SauceEditor.Views.Custom;
using SauceEditor.Views.Factories;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SauceEditor.ViewModels.Tools
{
    public class VolumeToolPanelViewModel : DockableViewModel
    {
        private List<VolumeTool> _children = new List<VolumeTool>();

        [PropagateChanges]
        [DoNotCheckEquality]
        public VolumeTool Tool { get; set; }

        public object Selection { get; set; }

        public VolumeToolPanelViewModel()
        {
            _children.Add(new BoxVolumeTool());
            _children.Add(new ConeVolumeTool());
            _children.Add(new CylinderVolumeTool());
            _children.Add(new SphereVolumeTool());

            Children = new ObservableCollection<VolumeTool>(_children);
        }

        public IEntityFactory EntityFactory { get; set; }

        public ObservableCollection<VolumeTool> Children { get; set; }
    }
}