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
using Graphics.GameObjects;
using Graphics.Outputs;

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

        public Vector3 Direction => (new Vector4(0.0f, 0.0f, -Height, 1.0f) * Matrix4.CreateFromQuaternion(Rotation)).Xyz;
        public Matrix4 Model => Matrix4.Identity * Matrix4.CreateScale(new Vector3(Radius, Radius, Height)) * Matrix4.CreateFromQuaternion(Rotation) * Matrix4.CreateTranslation(Position);
        public Matrix4 View => Matrix4.LookAt(Position, Position + Direction.Normalized(), Vector3.UnitZ);
        //public Matrix4 Projection => Matrix4.CreatePerspectiveFieldOfView((float)Math.Atan2(Radius, Height) * 2.0f, 1.0f, 0.1f, Height);

        public Matrix4 GetProjection(Resolution resolution)
        {
            var fovY = (float)Math.Atan2(Radius, Height) * 2.0f;
            return Matrix4.CreatePerspectiveFieldOfView(fovY, resolution.AspectRatio, 0.1f, Height);
        }

        public override void DrawForStencilPass(ShaderProgram program)
        {
            program.SetUniform("modelMatrix", Model);
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

        public override void DrawForLightPass(Resolution resolution, ShaderProgram program)
        {
            program.SetUniform("modelMatrix", Model);
            program.SetUniform("lightMatrix", View * GetProjection(resolution));
            program.SetUniform("lightViewMatrix", View);
            program.SetUniform("lightProjectionMatrix", GetProjection(resolution));

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
