using System;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SpiceEngineCore.Rendering.Matrices
{
    public class ModelMatrix : TransformMatrix
    {
        public const string CURRENT_NAME = "modelMatrix";
        public const string PREVIOUS_NAME = "previousModelMatrix";

        public ModelMatrix() : this(new Transform()) { }
        public ModelMatrix(Vector3 position, Quaternion rotation, Vector3 scale) : this(new Transform(position, rotation, scale)) { }
        public ModelMatrix(Transform transform)
        {
            WorldTransform = transform;
            InitializeValue(WorldTransform.ToMatrix());
        }

        public Transform WorldTransform { get; private set; }

        public Vector3 Position
        {
            get => WorldTransform.Translation;
            set => Transform(Matrices.Transform.FromTranslation(value - Position));
        }

        public Quaternion Rotation
        {
            get => WorldTransform.Rotation;
            //set => Transform(Matrices.Transform.FromRotation(value * Rotation.Inverted()));
            set
            {
                var rotationDifference = (value * Rotation.Inverted()).Normalized();
                Transform(Matrices.Transform.FromRotation(rotationDifference));
            }
        }

        public Vector3 Scale
        {
            get => WorldTransform.Scale;
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
            WorldTransform.Combine(transform);

            UpdateValue(WorldTransform.ToMatrix());
        }
    }
}
