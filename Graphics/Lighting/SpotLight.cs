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
using Graphics.Utilities;

namespace Graphics.Lighting
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

    public class SpotLight : Light
    {
        public Vector3 Position { get; set; }
        public float Radius { get; set; }
        public Quaternion Rotation { get; set; }
        public float Height { get; set; }

        public override void Draw(ShaderProgram program)
        {
            // When we apply the scale to the cone model, we need to translate it to the correct position before rotating
            var model = Matrix4.Identity
                * Matrix4.CreateScale(new Vector3(Radius, Radius, Height))
                * Matrix4.CreateFromQuaternion(Rotation)
                * Matrix4.CreateTranslation(Position);
            program.SetUniform("modelMatrix", model);

            program.SetUniform("lightPosition", Position);
            program.SetUniform("lightRadius", Radius);
            program.SetUniform("lightHeight", Height);

            var lightVector = (new Vector4(0.0f, 0.0f, -Height, 1.0f) * Matrix4.CreateFromQuaternion(Rotation)).Xyz;
            program.SetUniform("lightVector", lightVector);

            var cutoffVector = new Vector2(Radius, Height);
            var cosAngle = Vector2.Dot(new Vector2(0, Height), cutoffVector) / (Height * cutoffVector.Length);
            program.SetUniform("lightCutoffAngle", cosAngle);

            program.SetUniform("lightColor", Color);
            program.SetUniform("lightIntensity", Intensity);
        }
    }
}
