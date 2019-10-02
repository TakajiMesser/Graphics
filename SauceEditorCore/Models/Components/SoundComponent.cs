using SpiceEngineCore.Sounds;
using System;

namespace SauceEditorCore.Models.Components
{
    public class SoundComponent : Component
    {
        public SoundComponent(string filePath) : base(filePath) { }

        public Sound Sound { get; set; }

        public override void Save() => throw new NotImplementedException();
        public override void Load() => throw new NotImplementedException();

        public static bool IsValidExtension(string extension) => extension == ".wav";
    }
}
