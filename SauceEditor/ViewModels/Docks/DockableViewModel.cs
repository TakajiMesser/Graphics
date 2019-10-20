using System;

namespace SauceEditor.ViewModels.Docks
{
    public abstract class DockableViewModel : ViewModel
    {
        private bool _isActive = false;

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
                        BecameActive?.Invoke(this, new EventArgs());
                    }
                }
            }
        }

        public event EventHandler<EventArgs> BecameActive;
    }
}
