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
    public abstract class TransformMatrix
    {
        public Matrix4 CurrentValue { get; private set; }
        public Matrix4 PreviousValue { get; private set; }

        protected void InitializeValue(Matrix4 value)
        {
            PreviousValue = value;
            CurrentValue = value;
        }

        protected void UpdateValue(Matrix4 value)
        {
            PreviousValue = CurrentValue;
            CurrentValue = value;
        }
    }
}
