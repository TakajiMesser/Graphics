﻿using OpenTK;

namespace SpiceEngineCore.Entities.Lights
{
    /// <summary>
    /// This struct is used by the Forward Renderer, in a uniform buffer
    /// </summary>
    public struct SLight
    {
        public Vector3 Position { get; private set; }
        public float Radius { get; private set; }
        public Vector3 Color { get; private set; }
        public float Intensity { get; private set; }

        public SLight(Vector3 position, float radius, Vector3 color, float intensity)
        {
            Position = position;
            Radius = radius;
            Color = color;
            Intensity = intensity;
        }
    }
}
