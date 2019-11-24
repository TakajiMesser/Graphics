using SpiceEngineCore.Maps;

namespace SauceEditor.ViewModels.Tools.Cameras
{
    public class PerspectiveCameraTool : CameraTool
    {
        public PerspectiveCameraTool() : base("Perspective") { }

        public float ZNear { get; set; }
        public float ZFar { get; set; }
        public float FieldOfViewY { get; set; }

        // TODO - Do we need to mark this inherited property as not browsable as well?
        public override MapCamera MapEntity => new MapCamera()
        {
            Type = SpiceEngineCore.Rendering.Matrices.ProjectionTypes.Perspective,
            ZNear = ZNear,
            ZFar = ZFar,
            FieldOfViewY = FieldOfViewY
        };
    }
}
