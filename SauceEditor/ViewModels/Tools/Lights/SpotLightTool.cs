using SpiceEngineCore.Maps;

namespace SauceEditor.ViewModels.Tools.Lights
{
    public class SpotLightTool : LightTool
    {
        public SpotLightTool() : base("Spot") { }

        public float Radius { get; set; } = 10.0f;

        public float Height { get; set; } = 10.0f;

        // TODO - Do we need to mark this inherited property as not browsable as well?
        public override MapLight MapEntity => new MapLight()
        {
            LightType = MapLight.LightTypes.Point,
            Radius = Radius,
            Height = Height,
            Color = Color,
            Intensity = Intensity
        };
    }
}
