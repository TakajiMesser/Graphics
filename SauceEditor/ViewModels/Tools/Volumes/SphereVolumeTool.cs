using SpiceEngineCore.Rendering.Models;

namespace SauceEditor.ViewModels.Tools.Volumes
{
    public class SphereVolumeTool : VolumeTool
    {
        public SphereVolumeTool() : base("Sphere") { }

        public float Radius { get; set; } = 10.0f;
        public int NumberOfSides { get; set; } = 10;

        public override ModelMesh MeshShape => ModelMesh.Sphere(Radius, NumberOfSides);
    }
}
