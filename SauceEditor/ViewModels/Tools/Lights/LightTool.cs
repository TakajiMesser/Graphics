using OpenTK.Graphics;
using SpiceEngineCore.Maps;
using System.ComponentModel;

namespace SauceEditor.ViewModels.Tools.Lights
{
    public abstract class LightTool : Tool<MapLight>
    {
        private RelayCommand _openCommand;

        public LightTool(string name) : base(name) { }

        public Color4 Color { get; set; }

        public float Intensity { get; set; }

        [Browsable(false)]
        public override RelayCommand OpenCommand => (_openCommand = _openCommand ?? new RelayCommand(
            p => { },
            p => true
        ));
    }
}
