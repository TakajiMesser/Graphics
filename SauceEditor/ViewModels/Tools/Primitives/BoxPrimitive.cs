﻿using SpiceEngineCore.Rendering.Models;

namespace SauceEditor.ViewModels.Tools.Primitives
{
    public class BoxPrimitive : Primitive
    {
        public BoxPrimitive() : base("Box") { }

        public float Width { get; set; } = 10.0f;
        public float Height { get; set; } = 10.0f;
        public float Depth { get; set; } = 10.0f;

        public override ModelMesh MeshShape => ModelMesh.Box(Width, Height, Depth);

        private RelayCommand _openCommand;
        public RelayCommand OpenCommand => (_openCommand = _openCommand ?? new RelayCommand(
            p => { },
            p => true
        ));
    }
}
