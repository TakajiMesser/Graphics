using SpiceEngine.Maps;

namespace SauceEditor.ViewModels.Behaviors
{
    public class BehaviorViewModel : ViewModel, IMainDockViewModel
    {
        public bool IsPlayable => false;
        public bool IsActive { get; set; }
        public Map Map => null;
    }
}