using SpiceEngineCore.Maps;
using System.ComponentModel;

namespace SauceEditor.ViewModels.Tools.Cameras
{
    public abstract class CameraTool : Tool<MapCamera>
    {
        private RelayCommand _openCommand;

        public CameraTool(string name) : base(name) { }

        [Browsable(false)]
        public override RelayCommand OpenCommand => (_openCommand = _openCommand ?? new RelayCommand(
            p => { },
            p => true
        ));
    }
}
