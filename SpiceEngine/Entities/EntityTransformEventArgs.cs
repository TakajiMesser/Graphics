using OpenTK;
using SpiceEngine.Rendering.Matrices;
using System;

namespace SpiceEngine.Entities
{
    public class EntityTransformEventArgs : EventArgs
    {
        public int ID { get; }
        //public TransformTypes TransformType { get; }
        public Matrix4 Transform { get; }

        public EntityTransformEventArgs(int id, /*TransformTypes transformType, */Matrix4 transform)
        {
            ID = id;
            //TransformType = transformType;
            Transform = transform;
        }
    }
}
