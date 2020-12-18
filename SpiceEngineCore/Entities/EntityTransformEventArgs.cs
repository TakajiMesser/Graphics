using OpenTK;
using SpiceEngineCore.Rendering.Matrices;
using System;

namespace SpiceEngineCore.Entities
{
    public class EntityTransformEventArgs : EventArgs
    {
        public int ID { get; }
        public Vector3 Position { get; }
        public Transform Transform { get; }

        public EntityTransformEventArgs(int id, Vector3 position, Transform transform)
        {
            ID = id;
            Position = position;
            Transform = transform;
        }
    }
}
