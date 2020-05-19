using SpiceEngineCore.Maps;

namespace SauceEditor.ViewModels.Tools.Lights
{
    public class PointLightTool : LightTool
    {
        public PointLightTool() : base("Point") { }

        public float Radius { get; set; } = 10.0f;

        // TODO - Do we need to mark this inherited property as not browsable as well?
        public override MapLight MapEntity => new MapLight()
        {
            LightType = MapLight.LightTypes.Point,
            Radius = Radius,
            Color = Color,
            Intensity = Intensity
        };
    }
}
