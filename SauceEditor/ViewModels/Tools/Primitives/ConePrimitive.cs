using SpiceEngineCore.Rendering.Models;

namespace SauceEditor.ViewModels.Tools.Primitives
{
    public class ConePrimitive : Primitive
    {
        public ConePrimitive() : base("Cone") { }

        public float Radius { get; set; } = 10.0f;
        public float Height { get; set; } = 10.0f;
        public int NumberOfSides{ get; set; } = 10;

        public override ModelMesh MeshShape => ModelMesh.Cone(Radius, Height, NumberOfSides);

        private RelayCommand _openCommand;
        public RelayCommand OpenCommand => (_openCommand = _openCommand ?? new RelayCommand(
            p => { },
            p => true
        ));
    }
}
