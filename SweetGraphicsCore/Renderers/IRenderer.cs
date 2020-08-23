﻿using SpiceEngineCore.Rendering;

namespace SweetGraphicsCore.Renderers
{
    public interface IRenderer
    {
        void Load(Resolution resolution);
        void Resize(Resolution resolution);
    }
}