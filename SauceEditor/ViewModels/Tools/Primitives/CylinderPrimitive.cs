using SpiceEngineCore.Rendering.Models;

namespace SauceEditor.ViewModels.Tools.Primitives
{
    public class CylinderPrimitive : Primitive
    {
        public CylinderPrimitive() : base("Cylinder") { }

        public float Radius { get; set; } = 10.0f;
        public float Height { get; set; } = 10.0f;
        public int NumberOfSides { get; set; } = 10;

        public override ModelMesh MeshShape => ModelMesh.Cylinder(Radius, Height, NumberOfSides);

        private RelayCommand _openCommand;
        public RelayCommand OpenCommand => (_openCommand = _openCommand ?? new RelayCommand(
            p => { },
            p => true
        ));
    }
}
