using OpenTK;
using SpiceEngine.Rendering.Shaders;
using System;

namespace SpiceEngine.Entities.Lights
{
    public class SpotLight : Light<SLight>, IRotate
    {
        private float _radius;
        private float _height;

        public float Radius
        {
            get => _radius;
            set
            {
                _radius = value;
                _modelMatrix.Scale = new Vector3(_radius, _radius, _height);
            }
        }

        public float Height
        {
            get => _height;
            set
            {
                _height = value;
                _modelMatrix.Scale = new Vector3(_radius, _radius, _height);
            }
        }

        public Quaternion Rotation => _modelMatrix.Rotation;

        public void SetRotation(Quaternion rotation) => _modelMatrix.SetRotation(rotation);

        public Vector3 Direction => (new Vector4(0.0f, 0.0f, -Height, 1.0f) * Matrix4.CreateFromQuaternion(Rotation)).Xyz;

        public Matrix4 View => Matrix4.LookAt(Position, Position + Direction.Normalized(), Vector3.UnitZ);
        public Matrix4 Projection => Matrix4.CreatePerspectiveFieldOfView((float)Math.Atan2(_radius, Height) * 2.0f, 1.0f, 0.1f, Height);

        public override void DrawForStencilPass(ShaderProgram program)
        {
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
