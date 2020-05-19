using SpiceEngineCore.Outputs;

namespace SpiceEngine.Game.GameSettings
{
    public enum Quality
    {
        VeryLow,
        Low,
        Medium,
        High,
        VeryHigh
    }

    public class VideoSettings
    {
        public Resolution Resolution { get; set; }
        public bool IsFullScreen { get; set; }
        public Quality TextureQuality { get; set; }
        public Quality ModelQuality { get; set; }
    }
}
