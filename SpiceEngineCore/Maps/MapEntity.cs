using SpiceEngineCore.Entities;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Utilities;
using System;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

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
