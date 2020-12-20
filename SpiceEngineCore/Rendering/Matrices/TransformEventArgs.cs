using System;

namespace SpiceEngineCore.Rendering.Matrices
{
    public class TransformEventArgs : EventArgs
    {
        public TransformEventArgs(Transform transform) => Transform = transform;

        public Transform Transform { get; }
    }
}
