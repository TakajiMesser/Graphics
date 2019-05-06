using SauceEditor.Helpers;
using SauceEditor.ViewModels;
using SauceEditor.ViewModels.Commands;
using SauceEditor.Views.Factories;
using SpiceEngine.Maps;
using SpiceEngine.Outputs;
using SpiceEngine.Rendering.Processing;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.AvalonDock.Layout;
using ViewTypes = SpiceEngine.Game.ViewTypes;

namespace SauceEditor.Views.GamePanels
{
    /// <summary>
    /// Interaction logic for GamePanelManager.xaml
    /// </summary>
    public partial class GamePanelManager : LayoutAnchorable, IHaveViewModel
    {
        private GamePanelView _perspectiveView;
        private GamePanelView _xView;
        private GamePanelView _yView;
        private GamePanelView _zView;

        //public event EventHandler<EntitiesEventArgs> EntitySelectionChanged;
        //public event EventHandler<CommandEventArgs> CommandExecuted;

        public GamePanelManager()
        {
            InitializeComponent();
            ViewModel.Resolution = new Resolution((int)Panel.Width, (int)Panel.Height);

            CreateAndShowPanels();

            ViewModel.PerspectiveViewModel = _perspectiveView.ViewModel;
            ViewModel.XViewModel = _xView.ViewModel;
            ViewModel.YViewModel = _yView.ViewModel;
            ViewModel.ZViewModel = _zView.ViewModel;
        }

        public ViewModel GetViewModel() => ViewModel;

        private void MainDock_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    ViewModel.TransformMode = (TransformModes)((int)(ViewModel.TransformMode + 1) % Enum.GetValues(typeof(TransformModes)).Length);
                    break;
                case Key.Home:
                    ViewModel.CenterView();
                    break;
            }

            e.Handled = true;
        }

        private void CreateAndShowPanels()
        {
            _perspectiveView = CreatePanel(ViewTypes.Perspective, AnchorableShowStrategy.Most);
            _xView = CreatePanel(ViewTypes.X, AnchorableShowStrategy.Right);
            _yView = CreatePanel(ViewTypes.Y, AnchorableShowStrategy.Bottom);
            _zView = CreatePanel(ViewTypes.Z, AnchorableShowStrategy.Right | AnchorableShowStrategy.Bottom);

            DockHelper.AddPanesToDockAsGrid(MainDockingManager, 2, _perspectiveView, _xView, _yView, _zView);
        }

        private GamePanelView CreatePanel(ViewTypes viewType, AnchorableShowStrategy showStrategy)
        {
            var gamePanel = new GamePanelView();
            gamePanel.ViewModel.ViewType = viewType;
            gamePanel.Anchorable.Show();

            return gamePanel;
        }

        /*private void OnEntitySelectionChanged(ViewTypes viewType, SpiceEngine.Game.EntitiesEventArgs args)
        {
            if (viewType != ViewTypes.Perspective) _perspectiveView.Panel.SelectEntities(args.Entities);
            if (viewType != ViewTypes.X) _xView.Panel.SelectEntities(args.Entities);
            if (viewType != ViewTypes.Y) _yView.Panel.SelectEntities(args.Entities);
            if (viewType != ViewTypes.Z) _zView.Panel.SelectEntities(args.Entities);

            if (args.Entities.Count > 0)
            {
                if (viewType != ViewTypes.Perspective) _perspectiveView.Panel.UpdateEntities(args.Entities);
                if (viewType != ViewTypes.X) _xView.Panel.UpdateEntities(args.Entities);
                if (viewType != ViewTypes.Y) _yView.Panel.UpdateEntities(args.Entities);
                if (viewType != ViewTypes.Z) _zView.Panel.UpdateEntities(args.Entities);

                ViewModel.MapManager.UpdateEntities(args.Entities);
            }

            var editorEntities = ViewModel.MapManager.GetEditorEntities(args.Entities);
            EntitySelectionChanged?.Invoke(this, new EntitiesEventArgs(editorEntities));
            //EntitySelectionChanged?.Invoke(this, new EntitiesEventArgs(_mapManager.GetMapEntities(args.Entities)));
        }*/

        public void SetView(Models.ViewTypes view)
        {
            switch (view)
            {
                case Models.ViewTypes.All:
                    View_All.IsSelected = true;
                    break;
                case Models.ViewTypes.Perspective:
                    View_Perspective.IsSelected = true;
                    break;
                case Models.ViewTypes.X:
                    View_X.IsSelected = true;
                    break;
                case Models.ViewTypes.Y:
                    View_Y.IsSelected = true;
                    break;
                case Models.ViewTypes.Z:
                    View_Z.IsSelected = true;
                    break;
            }
        }

        private void ViewComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = ViewComboBox.SelectedItem as ComboBoxItem;
            
            switch (selectedItem.Content)
            {
                case "All":
                    DockHelper.ShowAllPanesInDockAsGrid(MainDockingManager);
                    break;
                case "Perspective":
                    DockHelper.ShowSinglePaneInDockGrid(MainDockingManager, _perspectiveView);
                    break;
                case "X":
                    DockHelper.ShowSinglePaneInDockGrid(MainDockingManager, _xView);
                    break;
                case "Y":
                    DockHelper.ShowSinglePaneInDockGrid(MainDockingManager, _yView);
                    break;
                case "Z":
                    DockHelper.ShowSinglePaneInDockGrid(MainDockingManager, _zView);
                    break;
            }
        }
    }
}
