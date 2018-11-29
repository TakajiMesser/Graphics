using OpenTK;
using System;
using System.Linq;
using SpiceEngine.Entities;
using System.Collections.Generic;

namespace SpiceEngine.Physics.Collision
{
    public class BoundingBox : Bounds
    {
        public float Width { get; set; }
        public float Height { get; set; }

        public float MinX => Center.X - Width / 2.0f;
        public float MaxX => Center.X + Width / 2.0f;
        public float MinY => Center.Y - Height / 2.0f;
        public float MaxY => Center.Y + Height / 2.0f;

        public BoundingBox(IEntity entity, IEnumerable<Vector3> vertices) : base(entity)
        {
            IEnumerable<Vector3> adjustedVertices;

            if (entity is Actor actor)
            {
                adjustedVertices = vertices.Select(v => (Matrix4.CreateScale(actor.Scale) * Matrix4.CreateFromQuaternion(actor.Rotation) * new Vector4(v, 1.0f)).Xyz);
            }
            else
            {
                adjustedVertices = vertices;
            }

            var minX = adjustedVertices.Select(v => v.X).Min();
            var maxX = adjustedVertices.Select(v => v.X).Max();
            Width = maxX - minX;

            var minY = adjustedVertices.Select(v => v.Y).Min();
            var maxY = adjustedVertices.Select(v => v.Y).Max();
            Height = maxY - minY;
        }

        public override bool CollidesWith(Vector3 point) => (point.X > MinX && point.X < MaxX) && (point.Y > MinY && point.Y < MaxY);

        public override bool CollidesWith(Bounds collider)
        {
            switch (collider)
            {
                case BoundingCircle c:
                    return CollidesWith(c);
                case BoundingBox b:
                    return CollidesWith(b);
                default:
                    throw new NotImplementedException();
            }
        }

        public override bool CollidesWith(BoundingCircle boundingCircle) => HasCollision(boundingCircle, this);

        public override bool CollidesWith(BoundingBox boundingBox) =>
            (MinX < boundingBox.MaxX && MaxX > boundingBox.MinX) && (MinY < boundingBox.MaxY && MaxY > boundingBox.MinY);

        public override Vector3 GetBorder(Vector3 direction)
        {
            var xRatio = (Width / 2.0f) / direction.X;
            var yRatio = (Height / 2.0f) / direction.Y;

            var newX = direction.X * yRatio;
            var newY = direction.Y * xRatio;

            return (Math.Abs(newX) < Width / 2.0f)
                ? new Vector3(Center.X + newX, Center.Y + direction.Y * yRatio, Center.Z)
                : new Vector3(Center.X + direction.X * xRatio, Center.Y + newY, Center.Z);
        }
    }
}
