using SauceEditor.Helpers;
using SauceEditor.Models;
using SpiceEngine.Rendering.Processing;
using SpiceEngineCore.Outputs;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.AvalonDock.Layout;
using ViewTypes = SpiceEngine.Game.ViewTypes;

namespace SauceEditor.Views.GamePanels
{
    /// <summary>
    /// Interaction logic for GamePanel.xaml
    /// </summary>
    public partial class GamePanel : LayoutAnchorable
    {
        private GamePane _perspectivePane;
        private GamePane _xPane;
        private GamePane _yPane;
        private GamePane _zPane;

        //public event EventHandler<EntitiesEventArgs> EntitySelectionChanged;
        //public event EventHandler<CommandEventArgs> CommandExecuted;

        public GamePanel()
        {
            InitializeComponent();
            ViewModel.Resolution = new Resolution((int)Panel.Width, (int)Panel.Height);

            CreateAndShowPanes();

            ViewModel.PerspectiveViewModel = _perspectivePane.ViewModel;
            ViewModel.XViewModel = _xPane.ViewModel;
            ViewModel.YViewModel = _yPane.ViewModel;
            ViewModel.ZViewModel = _zPane.ViewModel;

            //SetView(Models.ViewTypes.Y);
            SetView(EditorSettings.Instance.DefaultView);
        }

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

        private void CreateAndShowPanes()
        {
            _perspectivePane = CreatePane(ViewTypes.Perspective, AnchorableShowStrategy.Most);
            _xPane = CreatePane(ViewTypes.X, AnchorableShowStrategy.Right);
            _yPane = CreatePane(ViewTypes.Y, AnchorableShowStrategy.Bottom);
            _zPane = CreatePane(ViewTypes.Z, AnchorableShowStrategy.Right | AnchorableShowStrategy.Bottom);

            //DockHelper.AddPanesToDockAsGrid(MainDockingManager, 1, _perspectivePane);
            DockHelper.AddPanesToDockAsGrid(MainDockingManager, 2, _perspectivePane, _xPane, _yPane, _zPane);
        }

        private GamePane CreatePane(ViewTypes viewType, AnchorableShowStrategy showStrategy)
        {
            var gamePane = new GamePane();
            gamePane.ViewModel.ViewType = viewType;

            // TODO - This isn't MVVM
            switch (viewType)
            {
                case ViewTypes.Perspective:
                    gamePane.Anchor.Title = "Perspective";
                    break;
                case ViewTypes.X:
                    gamePane.Anchor.Title = "X";
                    break;
                case ViewTypes.Y:
                    gamePane.Anchor.Title = "Y";
                    break;
                case ViewTypes.Z:
                    gamePane.Anchor.Title = "Z";
                    break;
                default:
                    throw new ArgumentException("Could not handle ViewType " + viewType);
            }

            gamePane.Anchor.Show();
            return gamePane;
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
                    DockHelper.ShowSinglePaneInDockGrid(MainDockingManager, _perspectivePane);
                    break;
                case "X":
                    DockHelper.ShowSinglePaneInDockGrid(MainDockingManager, _xPane);
                    break;
                case "Y":
                    DockHelper.ShowSinglePaneInDockGrid(MainDockingManager, _yPane);
                    break;
                case "Z":
                    DockHelper.ShowSinglePaneInDockGrid(MainDockingManager, _zPane);
                    break;
            }
        }
    }
}
