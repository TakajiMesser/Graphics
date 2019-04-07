using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Utilities;

namespace SpiceEngine.Maps
{
    public abstract class MapEntity3D<T> : IMapEntity3D where T : IEntity
    {
        public Vector3 Position { get; set; } = Vector3.Zero;
        public Vector3 Rotation { get; set; } = Vector3.Zero;
        public Vector3 Scale { get; set; } = Vector3.One;

        public abstract T ToEntity();

        public virtual void UpdateFrom(T entity)
        {
            /*Position = entity.Position;

            if (entity is IRotate rotator)
            {
                Rotation = rotator.Rotation.ToEulerAngles();
            }

            if (entity is IScale scaler)
            {
                Scale = scaler.Scale;
            }*/
        }
    }
}
