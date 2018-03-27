using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using TakoEngine.Outputs;
using TakoEngine.Rendering.Shaders;
using TakoEngine.Utilities;

namespace TakoEngine.Entities.Lights
{
    /// <summary>
    /// This struct is used by the Forward Renderer, in a uniform buffer
    /// </summary>
    public struct DLight
    {
        public Vector3 Color { get; private set; }
        public float Intensity { get; private set; }

        public DLight(Vector3 color, float intensity)
        {
            Color = color;
            Intensity = intensity;
        }
    }
}
