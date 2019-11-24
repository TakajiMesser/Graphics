using OpenTK;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Utilities;
using System;

namespace SpiceEngineCore.Maps
{
    public abstract class MapEntity<T> : IEntityBuilder, IMapEntity where T : IEntity
    {
        public Vector3 Position { get; set; } = Vector3.Zero;
        public Vector3 Rotation { get; set; } = Vector3.Zero;
        public Vector3 Scale { get; set; } = Vector3.One;

        public abstract IEntity ToEntity();

        public Type GetEntityType() => typeof(T);

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
