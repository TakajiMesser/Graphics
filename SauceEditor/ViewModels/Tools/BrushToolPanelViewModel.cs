using PropertyChanged;
using SauceEditor.ViewModels.Docks;
using SauceEditor.ViewModels.Tools.Brushes;
using SauceEditor.Views.Custom;
using SauceEditor.Views.Factories;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SauceEditor.ViewModels.Tools
{
    public class BrushToolPanelViewModel : DockableViewModel
    {
        private List<BrushTool> _children = new List<BrushTool>();

        [PropagateChanges]
        [DoNotCheckEquality]
        public BrushTool Tool { get; set; }

        public object Selection { get; set; }

        public BrushToolPanelViewModel()
        {
            _children.Add(new BoxBrushTool());
            _children.Add(new ConeBrushTool());
            _children.Add(new CylinderBrushTool());
            _children.Add(new SphereBrushTool());

            Children = new ObservableCollection<BrushTool>(_children);
        }

        public IEntityFactory EntityFactory { get; set; }

        public ObservableCollection<BrushTool> Children { get; set; }
    }
}