using SpiceEngineCore.Rendering.Models;

namespace SauceEditor.ViewModels.Tools.Volumes
{
    public class ConeVolumeTool : VolumeTool
    {
        public ConeVolumeTool() : base("Cone") { }

        public float Radius { get; set; } = 10.0f;
        public float Height { get; set; } = 10.0f;
        public int NumberOfSides{ get; set; } = 10;

        public override ModelMesh MeshShape => ModelMesh.Cone(Radius, Height, NumberOfSides);
    }
}
