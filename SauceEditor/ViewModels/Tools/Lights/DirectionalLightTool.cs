using SpiceEngineCore.Maps;

namespace SauceEditor.ViewModels.Tools.Lights
{
    public class DirectionalLightTool : LightTool
    {
        public DirectionalLightTool() : base("Directional") { }

        // TODO - Do we need to mark this inherited property as not browsable as well?
        public override MapLight MapEntity => new MapLight()
        {
            LightType = MapLight.LightTypes.Directional,
            Color = Color,
            Intensity = Intensity
        };
    }
}
