using SauceEditor.Views;
using SauceEditor.Views.Factories;

namespace SauceEditor.ViewModels.Docks
{
    public abstract class DockViewModel : ViewModel
    {
        private bool _isActive = false;

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
                        //DockTracker?.(this);
                    }
                }
            }
        }
    }
}
