using SauceEditor.Helpers;
using SauceEditor.ViewModels.Docks;
using System.Collections.Generic;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;

namespace SauceEditor.Views
{
    public class DockTracker : IDockTracker
    {
        private DockingManager _leftDockingManager;
        private DockingManager _centerDockingManager;
        private DockingManager _rightDockingManager;

        private DockableViewModel _activeLeftDockVM;
        private DockableViewModel _activeCenterDockVM;
        private DockableViewModel _activeRightDockVM;

        private Dictionary<DockableViewModel, LayoutAnchorable> _leftDockViewByVM = new Dictionary<DockableViewModel, LayoutAnchorable>();
        private Dictionary<DockableViewModel, LayoutAnchorable> _centerDockViewByVM = new Dictionary<DockableViewModel, LayoutAnchorable>();
        private Dictionary<DockableViewModel, LayoutAnchorable> _rightDockViewByVM = new Dictionary<DockableViewModel, LayoutAnchorable>();

        public DockTracker(DockingManager leftDockingManager, DockingManager centerDockingManager, DockingManager rightDockingManager)
        {
            _leftDockingManager = leftDockingManager;
            _centerDockingManager = centerDockingManager;
            _rightDockingManager = rightDockingManager;
        }

        public DockableViewModel ActiveLeftDockVM
        {
            get => _activeLeftDockVM;
            set
            {
                if (!_leftDockViewByVM.ContainsKey(value)) throw new KeyNotFoundException("ViewModel not found in dock");

                _leftDockViewByVM[value].IsActive = true;
                _activeLeftDockVM = value;
            }
        }

        public DockableViewModel ActiveCenterDockVM
        {
            get => _activeCenterDockVM;
            set
            {
                if (!_centerDockViewByVM.ContainsKey(value)) throw new KeyNotFoundException("ViewModel not found in dock");

                _centerDockViewByVM[value].IsActive = true;
                _activeCenterDockVM = value;
            }
        }

        public DockableViewModel ActiveRightDockVM
        {
            get => _activeRightDockVM;
            set
            {
                if (!_rightDockViewByVM.ContainsKey(value)) throw new KeyNotFoundException("ViewModel not found in dock");

                _rightDockViewByVM[value].IsActive = true;
                _activeRightDockVM = value;
            }
        }

        public void AddToLeftDock(LayoutAnchorable view, DockableViewModel viewModel)
        {
            view.IsActiveChanged += (s, args) => viewModel.IsActive = view.IsActive;
            viewModel.BecameActive += (s, args) => _activeLeftDockVM = viewModel;

            _leftDockViewByVM.Add(viewModel, view);
            DockHelper.AddToDockAsDocument(_leftDockingManager, view);
        }

        public void AddToCenterDock(LayoutAnchorable view, DockableViewModel viewModel)
        {
            view.IsActiveChanged += (s, args) => viewModel.IsActive = view.IsActive;
            viewModel.BecameActive += (s, args) => _activeCenterDockVM = viewModel;

            _centerDockViewByVM.Add(viewModel, view);
            DockHelper.AddToDockAsDocument(_centerDockingManager, view);
        }

        public void AddToRightDock(LayoutAnchorable view, DockableViewModel viewModel)
        {
            view.IsActiveChanged += (s, args) => viewModel.IsActive = view.IsActive;
            viewModel.BecameActive += (s, args) => _activeRightDockVM = viewModel;

            _rightDockViewByVM.Add(viewModel, view);
            DockHelper.AddToDockAsDocument(_rightDockingManager, view);
        }
    }
}
