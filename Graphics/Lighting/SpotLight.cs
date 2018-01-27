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
    public class SpotLight : Light
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public float Cutoff { get; set; }
        public Vector2 Width { get; set; }

        public override void Draw(ShaderProgram program)
        {
            // Need to set the model matrix (?)
            var model = Matrix4.Identity * Matrix4.CreateScale(new Vector3(Width.X, Width.Y, Cutoff)) * Matrix4.CreateFromQuaternion(Rotation) * Matrix4.CreateTranslation(Position);
            program.SetUniform("modelMatrix", model);

            program.SetUniform("lightPosition", Position);
            program.SetUniform("lightCutoff", Cutoff);

            var lightVector = new Vector3(0.0f, 0.0f, Cutoff);
            var cutoffVector = new Vector3(Width.X, Width.Y, Cutoff);
            var cosAngle = Vector3.Dot(lightVector, cutoffVector) / (lightVector.Length * cutoffVector.Length);

            program.SetUniform("lightCutoffAngle", cosAngle);
            program.SetUniform("lightColor", Color);
            program.SetUniform("lightIntensity", Intensity);
        }
    }
}
