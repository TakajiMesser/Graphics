using SpiceEngineCore.Rendering.Models;

namespace SauceEditor.ViewModels.Tools.Volumes
{
    public class CylinderVolumeTool : VolumeTool
    {
        public CylinderVolumeTool() : base("Cylinder") { }

        public float Radius { get; set; } = 10.0f;
        public float Height { get; set; } = 10.0f;
        public int NumberOfSides { get; set; } = 10;

        public override ModelMesh MeshShape => ModelMesh.Cylinder(Radius, Height, NumberOfSides);
    }
}
