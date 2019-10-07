using SauceEditor.ViewModels.Docks;
using Xceed.Wpf.AvalonDock.Layout;

namespace SauceEditor.Views.Trees.Entities
{
    /// <summary>
    /// Interaction logic for EntityTreePanel.xaml
    /// </summary>
    public partial class EntityTreePanel : LayoutAnchorable, IHaveDockViewModel
    {
        /*public event EventHandler<ItemSelectedEventArgs> MapSelected;
        public event EventHandler<ItemSelectedEventArgs> ModelSelected;
        public event EventHandler<ItemSelectedEventArgs> BehaviorSelected;
        public event EventHandler<ItemSelectedEventArgs> TextureSelected;
        public event EventHandler<ItemSelectedEventArgs> AudioSelected;

        private Project _project;*/

        public EntityTreePanel()
        {
            InitializeComponent();
            //ClearProject();
        }

        public DockViewModel GetViewModel() => ViewModel;
    }
}
