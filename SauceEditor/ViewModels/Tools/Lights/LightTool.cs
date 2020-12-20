using SpiceEngineCore.Maps;
using System.ComponentModel;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

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
