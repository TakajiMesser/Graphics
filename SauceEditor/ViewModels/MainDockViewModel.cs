using SauceEditor.Models;
using SauceEditor.Views.Factories;
using SpiceEngine.Maps;

namespace SauceEditor.ViewModels
{
    public abstract class MainDockViewModel : ViewModel
    {
        private bool _isActive = false;

        public IMainViewFactory MainViewFactory { get; set; }

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
                        MainViewFactory?.SetActiveInMainDock(this);
                    }
                }
            }
        }

        protected MapManager MapManager { get; set; }
        public Map Map => MapManager?.Map;
    }
}
