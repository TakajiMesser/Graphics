using OpenTK;
using SpiceEngine.Rendering.Shaders;
using System;

namespace SpiceEngine.Entities.Lights
{
    public class SpotLight : Light<SLight>, IRotate
    {
        public float Radius { get; set; }
        public float Height { get; set; }

        public Quaternion Rotation { get; set; }

        public Vector3 Direction => (new Vector4(0.0f, 0.0f, -Height, 1.0f) * Matrix4.CreateFromQuaternion(Rotation)).Xyz;
        public Matrix4 Model => Matrix4.Identity * Matrix4.CreateScale(new Vector3(Radius, Radius, Height)) * Matrix4.CreateFromQuaternion(Rotation) * Matrix4.CreateTranslation(Position);
        public Matrix4 View => Matrix4.LookAt(Position, Position + Direction.Normalized(), Vector3.UnitZ);
        public Matrix4 Projection => Matrix4.CreatePerspectiveFieldOfView((float)Math.Atan2(Radius, Height) * 2.0f, 1.0f, 0.1f, Height);

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

        public override void DrawForLightPass(ShaderProgram program)
        {
            program.SetUniform("modelMatrix", Model);
            program.SetUniform("lightMatrix", View * Projection);

            program.SetUniform("lightPosition", Position);
            program.SetUniform("lightRadius", Radius);
            program.SetUniform("lightHeight", Height);

            var lightVector = (new Vector4(0.0f, 0.0f, -Height, 1.0f) * Matrix4.CreateFromQuaternion(Rotation)).Xyz;
            program.SetUniform("lightVector", lightVector);

            var cutoffVector = new Vector2(Radius, Height);
            var cosAngle = Vector2.Dot(new Vector2(0, Height), cutoffVector) / (Height * cutoffVector.Length);
            program.SetUniform("lightCutoffAngle", cosAngle);

            program.SetUniform("lightColor", Color.Xyz);
            program.SetUniform("lightIntensity", Intensity);
        }

        public override SLight ToStruct() => new SLight(Position, Radius, Color.Xyz, Intensity);
    }
}
