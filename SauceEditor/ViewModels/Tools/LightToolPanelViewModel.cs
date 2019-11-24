using PropertyChanged;
using SauceEditor.ViewModels.Docks;
using SauceEditor.ViewModels.Tools.Lights;
using SauceEditor.Views.Custom;
using SauceEditor.Views.Factories;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SauceEditor.ViewModels.Tools
{
    public class LightToolPanelViewModel : DockableViewModel
    {
        private List<LightTool> _children = new List<LightTool>();

        [PropagateChanges]
        [DoNotCheckEquality]
        public LightTool Tool { get; set; }

        public object Selection { get; set; }

        public LightToolPanelViewModel()
        {
            _children.Add(new PointLightTool());
            _children.Add(new SpotLightTool());
            _children.Add(new DirectionalLightTool());

            Children = new ObservableCollection<LightTool>(_children);
        }

        public IEntityFactory EntityFactory { get; set; }

        public ObservableCollection<LightTool> Children { get; set; }
    }
}