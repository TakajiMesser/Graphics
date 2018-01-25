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
    public struct PointLight
    {
        public Vector3 Position { get; set; }
        public float Radius { get; set; }
        public Vector3 Color { get; set; }
        public float Intensity { get; set; }

        public void Draw(ShaderProgram program)
        {
            // Need to set the model matrix (?)
            var model = Matrix4.Identity
                * Matrix4.CreateScale(Radius)
                * Matrix4.CreateFromQuaternion(Quaternion.Identity)
                * Matrix4.CreateTranslation(Position);
            program.SetUniform("modelMatrix", model);

            program.SetUniform("lightPosition", Position);
            program.SetUniform("lightRadius", Radius);
            program.SetUniform("lightColor", Color);
            program.SetUniform("lightIntensity", Intensity);
        }
    }
}
