using SauceEditor.Views;

namespace SauceEditor.ViewModels.Docks
{
    public abstract class DockViewModel : ViewModel
    {
        public enum DockTypes
        {
            Game,
            Property,
            Tool
        }

        private bool _isActive = false;

        public DockViewModel(DockTypes dockType) => DockType = dockType;

        public DockTypes DockType { get; }
        public ITrackDocks DockTracker { get; set; }

        public bool IsPlayable { get; protected set; }
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;

                    if (_isActive)
                    {
                        if (DockTracker != null)
                        {
                            switch (DockType)
                            {
                                case DockTypes.Game:
                                    DockTracker.ActiveGameDockVM = this;
                                    break;
                                case DockTypes.Property:
                                    DockTracker.ActivePropertyDockVM = this;
                                    break;
                                case DockTypes.Tool:
                                    DockTracker.ActiveToolDockVM = this;
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
}
