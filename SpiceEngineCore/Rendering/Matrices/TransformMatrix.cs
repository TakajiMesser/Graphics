using OpenTK;

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
