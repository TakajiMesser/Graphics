using System;

namespace SpiceEngine.Rendering.Matrices
{
    public class TransformEventArgs : EventArgs
    {
        public Transform Transform { get; private set; }

        public TransformEventArgs(Transform transform) => Transform = transform;
    }
}
