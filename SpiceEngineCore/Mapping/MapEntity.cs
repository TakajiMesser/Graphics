using SpiceEngineCore.Entities;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Utilities;
using System;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

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
