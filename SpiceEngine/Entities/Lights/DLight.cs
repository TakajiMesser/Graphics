using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using SpiceEngine.Outputs;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Utilities;

namespace SpiceEngine.Entities.Lights
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
