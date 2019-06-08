using SauceEditor.Helpers;
using SauceEditor.ViewModels;
using SauceEditor.ViewModels.Docks;
using System.Collections.Generic;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;

namespace SauceEditor.Views
{
    public class DockTracker : ITrackDocks
    {
        private DockingManager _gameDockingManager;
        private DockingManager _propertyDockingManager;
        private DockingManager _toolDockingManager;

        private DockViewModel _activeGameDockVM;
        private DockViewModel _activePropertyDockVM;
        private DockViewModel _activeToolDockVM;

        private Dictionary<DockViewModel, LayoutAnchorable> _gameDockViewByVM = new Dictionary<DockViewModel, LayoutAnchorable>();
        private Dictionary<DockViewModel, LayoutAnchorable> _propertyDockViewByVM = new Dictionary<DockViewModel, LayoutAnchorable>();
        private Dictionary<DockViewModel, LayoutAnchorable> _toolDockViewByVM = new Dictionary<DockViewModel, LayoutAnchorable>();

        public DockTracker(DockingManager gameDockingManager, DockingManager propertyDockingManager, DockingManager toolDockingManager)
        {
            _gameDockingManager = gameDockingManager;
            _propertyDockingManager = propertyDockingManager;
            _toolDockingManager = toolDockingManager;
        }

        public void AddToGameDock<T>(T view) where T : LayoutAnchorable, IHaveDockViewModel
        {
            var dockViewModel = view.GetViewModel();
            view.IsActiveChanged += (s, args) => dockViewModel.IsActive = view.IsActive;

            _gameDockViewByVM.Add(dockViewModel, view);
            DockHelper.AddToDockAsDocument(_gameDockingManager, view);
        }

        public void AddToPropertyDock<T>(T view) where T : LayoutAnchorable, IHaveDockViewModel
        {
            var dockViewModel = view.GetViewModel();
            view.IsActiveChanged += (s, args) => dockViewModel.IsActive = view.IsActive;

            _propertyDockViewByVM.Add(dockViewModel, view);
            DockHelper.AddToDockAsDocument(_propertyDockingManager, view);
        }

        public void AddToToolDock<T>(T view) where T : LayoutAnchorable, IHaveDockViewModel
        {
            var dockViewModel = view.GetViewModel();
            view.IsActiveChanged += (s, args) => dockViewModel.IsActive = view.IsActive;

            _toolDockViewByVM.Add(dockViewModel, view);
            DockHelper.AddToDockAsDocument(_toolDockingManager, view);
        }

        public MainWindowViewModel MainWindowVM { get; set; }

        public DockViewModel ActiveGameDockVM
        {
            get => _activeGameDockVM;
            set
            {
                if (!_gameDockViewByVM.ContainsKey(value)) throw new KeyNotFoundException("ViewModel not found in dock");

                _gameDockViewByVM[value].IsActive = true;
                _activeGameDockVM = value;
                //MainWindowVM.CurrentMainDockViewModel = value;
            }
        }

        public DockViewModel ActivePropertyDockVM
        {
            get => _activePropertyDockVM;
            set
            {
                if (!_propertyDockViewByVM.ContainsKey(value)) throw new KeyNotFoundException("ViewModel not found in dock");

                _propertyDockViewByVM[value].IsActive = true;
                _activePropertyDockVM = value;
                //MainWindowVM.CurrentMainDockViewModel = value;
            }
        }

        public DockViewModel ActiveToolDockVM
        {
            get => _activeToolDockVM;
            set
            {
                if (!_toolDockViewByVM.ContainsKey(value)) throw new KeyNotFoundException("ViewModel not found in dock");

                _toolDockViewByVM[value].IsActive = true;
                _activeToolDockVM = value;
                //MainWindowVM.CurrentMainDockViewModel = value;
            }
        }
    }
}
