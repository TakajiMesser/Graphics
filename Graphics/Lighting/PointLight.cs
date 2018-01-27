using Graphics.Meshes;
using Graphics.Rendering.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Lighting
{
    /// <summary>
    /// This struct is used by the Forward Renderer, in a uniform buffer
    /// </summary>
    public struct PLight
    {
        public Vector3 Position { get; private set; }
        public float Radius { get; private set; }
        public Vector3 Color { get; private set; }
        public float Intensity { get; private set; }

        public PLight(Vector3 position, float radius, Vector3 color, float intensity)
        {
            Position = position;
            Radius = radius;
            Color = color;
            Intensity = intensity;
        }
    }

    public class PointLight : Light
    {
        public Vector3 Position { get; set; }
        public float Radius { get; set; }

        public override void Draw(ShaderProgram program)
        {
            // Need to set the model matrix (?)
            var model = Matrix4.Identity * Matrix4.CreateScale(Radius) * Matrix4.CreateTranslation(Position);
            program.SetUniform("modelMatrix", model);

            program.SetUniform("lightPosition", Position);
            program.SetUniform("lightRadius", Radius);
            program.SetUniform("lightColor", Color);
            program.SetUniform("lightIntensity", Intensity);
        }

        public PLight ToStruct() => new PLight(Position, Radius, Color, Intensity);
    }
}
