using SpiceEngineCore.Rendering.Models;

namespace SauceEditor.ViewModels.Tools.Primitives
{
    public class SpherePrimitive : Primitive
    {
        public SpherePrimitive() : base("Sphere") { }

        public float Radius { get; set; } = 10.0f;
        public int NumberOfSides { get; set; } = 10;

        public override ModelMesh MeshShape => ModelMesh.Sphere(Radius, NumberOfSides);

        private RelayCommand _openCommand;
        public RelayCommand OpenCommand => (_openCommand = _openCommand ?? new RelayCommand(
            p => { },
            p => true
        ));
    }
}
