using SpiceEngineCore.Rendering.Models;

namespace SauceEditor.ViewModels.Tools.Brushes
{
    public class BoxBrushTool : BrushTool
    {
        public BoxBrushTool() : base("Box") { }

        public float Width { get; set; } = 10.0f;
        public float Height { get; set; } = 10.0f;
        public float Depth { get; set; } = 10.0f;

        public override ModelMesh MeshShape => ModelMesh.Box(Width, Height, Depth);
    }
}
