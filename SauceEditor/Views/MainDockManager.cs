using SauceEditor.Helpers;
using SauceEditor.Models.Components;
using SauceEditor.ViewModels;
using SauceEditor.ViewModels.Properties;
using SauceEditor.Views.Behaviors;
using SauceEditor.Views.Factories;
using SauceEditor.Views.GamePanels;
using SauceEditor.Views.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;

namespace SauceEditor.Views
{
    /*public class MainDockManager : IMainViewFactory
    {
        private DockingManager _dockingManager;
        private Dictionary<MainDockViewModel, LayoutAnchorable> _mainDockViewByViewModel = new Dictionary<MainDockViewModel, LayoutAnchorable>();

        public MainWindowViewModel MainWindowViewModel { get; set; }
        public MainDockViewModel CurrentMainDockViewModel { get; private set; }

        public MainDockManager(DockingManager dockingManager)
        {
            _dockingManager = dockingManager;
        }

        public ViewModel CreateGamePanelManager(MapComponent mapComponent, Component component = null)
        {
            var gamePanelManager = new GamePanelManager()
            {
                Title = mapComponent.Name,
                CanClose = true
            };
            /*gamePanelManager.EntitySelectionChanged += (s, args) =>
            {
                // For now, just go with the last entity that was selected
                //_propertyPanel.Entity = args.Entities.LastOrDefault();
                _propertyPanel.ViewModel.UpdateFromModel(args.Entities.LastOrDefault());
                _propertyPanel.IsActive = true;
                //SideDockManager.ActiveContent = _propertyPanel;
            };*
            //gamePanelManager.CommandExecuted += (s, args) => CommandExecuted(args.Command);
            gamePanelManager.ViewModel.MainViewFactory = this;
            gamePanelManager.ViewModel.UpdateFromModel(mapComponent.Map);
            gamePanelManager.IsActiveChanged += (s, args) =>
            {
                gamePanelManager.ViewModel.IsActive = gamePanelManager.IsActive;
                MainWindowViewModel.PropertyViewModel.InitializeProperties(component ?? mapComponent);
            };

            _mainDockViewByViewModel.Add(gamePanelManager.ViewModel, gamePanelManager);
            DockHelper.AddToDockAsDocument(_dockingManager, gamePanelManager/*, () => ViewModel.PlayVisibility = Visibility.Hidden*);

            //gamePanelManager.IsActive = true;

            /*MainDockingManager.ActiveContentChanged += (s, args) =>
            {
                if (MainDockingManager.ActiveContent == gamePanelManager)
                {
                    _propertyPanel.ViewModel = ;
                }
            };*

            //gamePanelManager.SetView(ViewModel.Settings.DefaultView);
            gamePanelManager.SetView(MainWindowViewModel.Settings.DefaultView);
            return gamePanelManager.ViewModel;
        }

        public ViewModel CreateScriptView(ScriptComponent scriptComponent)
        {
            var scriptView = new ScriptView()
            {
                Title = scriptComponent.Name,
                CanClose = true
            };
            scriptView.ViewModel.UpdateFromModel(scriptComponent);

            DockHelper.AddToDockAsDocument(_dockingManager, scriptView);
            _mainDockViewByViewModel.Add(scriptView.ViewModel, scriptView);

            return scriptView.ViewModel;
        }

        public ViewModel CreateBehaviorView(BehaviorComponent behaviorComponent)
        {
            var behaviorView = new BehaviorView()
            {
                Title = behaviorComponent.Name,
                CanClose = true
            };

            DockHelper.AddToDockAsDocument(_dockingManager, behaviorView);
            _mainDockViewByViewModel.Add(behaviorView.ViewModel, behaviorView);

            return behaviorView.ViewModel;

            // Temporary measures...
            /*if (_scriptView == null)
            {
                _scriptView = new ScriptView(filePath);
            }

            var anchorable = new LayoutAnchorable
            {
                Title = Path.GetFileNameWithoutExtension(filePath),
                Content = _scriptView,
                CanClose = true
            };
            anchorable.Closed += (s, args) =>
            {
                PlayButton.Visibility = Visibility.Hidden;
                _map = null;
            };
            anchorable.AddToLayout(MainDockingManager, AnchorableShowStrategy.Most);
            anchorable.DockAsDocument();*/

            /* if (_behaviorView == null)
            {
                _behaviorView = new BehaviorView();
            }*
        }

        public void SetActiveInMainDock(MainDockViewModel viewModel)
        {
            if (!_mainDockViewByViewModel.ContainsKey(viewModel))
            {
                throw new KeyNotFoundException("ViewModel not found in main dock");
            }

            _mainDockViewByViewModel[viewModel].IsActive = true;
            CurrentMainDockViewModel = viewModel;
            MainWindowViewModel.CurrentMainDockViewModel = viewModel;
        }
    }*/
}
