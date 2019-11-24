using PropertyChanged;
using SauceEditor.ViewModels.Docks;
using SauceEditor.ViewModels.Tools.Actors;
using SauceEditor.Views.Custom;
using SauceEditor.Views.Factories;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SauceEditor.ViewModels.Tools
{
    public class ActorToolPanelViewModel : DockableViewModel
    {
        private List<ActorTool> _children = new List<ActorTool>();

        [PropagateChanges]
        [DoNotCheckEquality]
        public ActorTool Tool { get; set; }

        public object Selection { get; set; }

        public ActorToolPanelViewModel()
        {
            Children = new ObservableCollection<ActorTool>(_children);
        }

        public IEntityFactory EntityFactory { get; set; }

        public ObservableCollection<ActorTool> Children { get; set; }
    }
}