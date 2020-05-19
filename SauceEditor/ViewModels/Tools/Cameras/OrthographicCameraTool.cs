using SpiceEngineCore.Maps;

namespace SauceEditor.ViewModels.Tools.Cameras
{
    public class OrthographicCameraTool : CameraTool
    {
        public OrthographicCameraTool() : base("Orthographic") { }

        public float ZNear { get; set; }
        public float ZFar { get; set; }
        public float StartingWidth { get; set; }

        // TODO - Do we need to mark this inherited property as not browsable as well?
        public override MapCamera MapEntity => new MapCamera()
        {
            Type = SpiceEngineCore.Rendering.Matrices.ProjectionTypes.Orthographic,
            ZNear = ZNear,
            ZFar = ZFar,
            StartingWidth = StartingWidth
        };
    }
}
