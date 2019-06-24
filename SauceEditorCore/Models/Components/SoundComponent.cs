using SpiceEngine.Sounds;
using System;
using System.Collections.Generic;
using System.Text;

namespace SauceEditorCore.Models.Components
{
    public class SoundComponent : Component
    {
        public SoundComponent(string filePath) : base(filePath) { }

        public Sound Sound { get; set; }

        public override void Save() => throw new NotImplementedException();
        public override void Load() => throw new NotImplementedException();
    }
}
