using SpiceEngine.Maps;

namespace SauceEditor.ViewModels
{
    public interface IMainDockViewModel
    {
        bool IsPlayable { get; }
        Map Map { get; }
    }
}
