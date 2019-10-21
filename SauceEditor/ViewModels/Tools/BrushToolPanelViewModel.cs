using SauceEditor.ViewModels.Docks;
using SauceEditor.ViewModels.Tools.Primitives;
using SauceEditor.Views.Factories;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SauceEditor.ViewModels.Tools
{
    public class BrushToolPanelViewModel : DockableViewModel
    {
        private PrimitiveManager _primitiveManager = new PrimitiveManager();
        private List<Primitive> _children = new List<Primitive>();

        public BrushToolPanelViewModel()
        {
            var primitives = _primitiveManager.GetPrimitives();
            _children.AddRange(primitives);

            Children = new ObservableCollection<Primitive>(_children);
        }

        public IEntityFactory EntityFactory { get; set; }

        public ObservableCollection<Primitive> Children { get; set; }
    }
}