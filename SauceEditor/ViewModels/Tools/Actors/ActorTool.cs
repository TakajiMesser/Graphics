using SpiceEngineCore.Maps;
using System.ComponentModel;

namespace SauceEditor.ViewModels.Tools.Actors
{
    public abstract class ActorTool : Tool<MapActor>
    {
        private RelayCommand _openCommand;

        public ActorTool(string name) : base(name) { }

        [Browsable(false)]
        public override RelayCommand OpenCommand => (_openCommand = _openCommand ?? new RelayCommand(
            p => { },
            p => true
        ));
    }
}
