using OpenTK;
using System;

namespace SpiceEngine.Entities
{
    public class EntityTransformEventArgs : EventArgs
    {
        public int ID { get; }
        public Matrix4 Transform { get; }

        public EntityTransformEventArgs(int id, Matrix4 transform)
        {
            ID = id;
            Transform = transform;
        }
    }
}
