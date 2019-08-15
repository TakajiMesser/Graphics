using OpenTK;
using System;

namespace SpiceEngine.Entities
{
    public class EntityTransformEventArgs : EventArgs
    {
        public int ID { get; }
        public Transform Transform { get; }

        public EntityTransformEventArgs(int id, Transform transform)
        {
            ID = id;
            Transform = transform;
        }
    }
}
