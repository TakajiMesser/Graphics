using System;
using System.Collections.Generic;
using System.Text;

namespace SauceEditorCore.Models.Components
{
    public class ArchetypeComponent : Component
    {
        public ArchetypeComponent(string filePath) : base(filePath) { }

        public override void Save() => throw new NotImplementedException();
        public override void Load() => throw new NotImplementedException();
    }
}
