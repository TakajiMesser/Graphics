using System;
using System.Collections.Generic;
using System.Text;

namespace SauceEditorCore.Models.Components
{
    public class BehaviorComponent : Component
    {
        public BehaviorComponent(string filePath) : base(filePath) { }

        public override void Save() => throw new NotImplementedException();
        public override void Load() => throw new NotImplementedException();

        public static bool IsValidExtension(string extension) => extension == ".beh";
    }
}
