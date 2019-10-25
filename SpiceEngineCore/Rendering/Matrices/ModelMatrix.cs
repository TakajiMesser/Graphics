using OpenTK;
using SpiceEngineCore.Rendering.Shaders;
using System;

namespace SpiceEngineCore.Rendering.Matrices
{
    public class ModelMatrix
    {
        public const string NAME = "modelMatrix";
        public const string PREVIOUS_NAME = "previousModelMatrix";

        private Transform _transform;

        public ModelMatrix() : this(new Transform()) { }
        public ModelMatrix(Vector3 position, Quaternion rotation, Vector3 scale) : this(new Transform(position, rotation, scale)) { }
        public ModelMatrix(Transform transform)
        {
            _transform = transform;
            CurrentMatrix = _transform.ToMatrix();
        }

        public Matrix4 CurrentMatrix { get; private set; }
        public Matrix4 PreviousMatrix { get; private set; }
        public Transform WorldTransform => _transform;

        public Vector3 Position
        {
            get => _transform.Translation;
            set => Transform(Matrices.Transform.FromTranslation(value - Position));
        }

        public Quaternion Rotation
        {
            get => _transform.Rotation;
            //set => Transform(Matrices.Transform.FromRotation(value * Rotation.Inverted()));
            set
            {
                var rotationDifference = (value * Rotation.Inverted()).Normalized();
                Transform(Matrices.Transform.FromRotation(rotationDifference));
            }
        }

        public Vector3 Scale
        {
            get => _transform.Scale;
            //set => Transform(Matrices.Transform.FromScale(value - Scale));
            set
            {
                // TODO - Because we are calculating differences on scale set, if the scale is ever zero'd out we cannot recover from it :(
                if (value.X == 0.0f || value.Y == 0.0f || value.Z == 0.0f) throw new NotSupportedException("Cannot handle a Scale value of zero!");

                var scaleDifference = new Vector3()
                {
                    X = value.X / Scale.X,
                    Y = value.Y / Scale.Y,
                    Z = value.Z / Scale.Z
                };

                Transform(Matrices.Transform.FromScale(scaleDifference));
            }
        }

        public event EventHandler<TransformEventArgs> Transformed;

        public void Transform(Transform transform)
        {
            Transformed?.Invoke(this, new TransformEventArgs(transform));
            _transform.Combine(transform);
            CurrentMatrix = _transform.ToMatrix();
        }

        public void Set(ShaderProgram program)
        {
            program.SetUniform(NAME, CurrentMatrix);
            program.SetUniform(PREVIOUS_NAME, PreviousMatrix);

            PreviousMatrix = CurrentMatrix;
        }
    }
}
