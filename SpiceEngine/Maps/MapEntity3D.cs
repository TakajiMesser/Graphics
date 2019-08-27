using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Maps.Builders;
using SpiceEngine.Utilities;

namespace SpiceEngine.Maps
{
    public abstract class MapEntity3D<T> : IEntityBuilder, IMapEntity3D where T : IEntity
    {
        public Vector3 Position { get; set; } = Vector3.Zero;
        public Vector3 Rotation { get; set; } = Vector3.Zero;
        public Vector3 Scale { get; set; } = Vector3.One;

        public abstract IEntity ToEntity();

        public virtual void UpdateFrom(T entity)
        {
            Position = entity.Position;

            if (entity is IRotate rotator)
            {
                Rotation = rotator.Rotation.ToEulerAngles().ToDegrees();
            }

            if (entity is IScale scaler)
            {
                Scale = scaler.Scale;
            }
        }
    }
}
