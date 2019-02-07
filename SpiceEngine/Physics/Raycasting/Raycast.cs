using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Physics.Shapes;
using System.Collections.Generic;

namespace SpiceEngine.Physics.Raycasting
{
    public static class Raycast
    {
        public static bool TryRaycast(Ray3 ray, IEnumerable<PhysicsBody> colliders, IEntityProvider entityProvider, out RaycastHit hit)
        {
            hit = new RaycastHit();
            float shortestDistance = ray.Distance;

            foreach (var collider in colliders)
            {
                var position = entityProvider.GetEntity(collider.EntityID).Position;

                if (collider.Shape.GetType() == typeof(Box))
                {
                    var box = (Box)collider.Shape;

                    if (ray.TryGetBoxIntersection(box, position, out Vector3 intersection))
                    {
                        float distance = (intersection - ray.Origin).Length;

                        if (hit.EntityID == 0|| distance < shortestDistance)
                        {
                            hit = new RaycastHit()
                            {
                                EntityID = collider.EntityID,
                                Intersection = intersection
                            };

                            shortestDistance = distance;
                        }
                    }
                }
                else if (collider.Shape.GetType() == typeof(RayCircle))
                {
                    var circle = (Circle)collider.Shape;

                    if (ray.TryGetCircleIntersection(circle, position, out Vector3 intersection))
                    {
                        float distance = (intersection - ray.Origin).Length;

                        if (hit.EntityID == 0 || distance < shortestDistance)
                        {
                            hit = new RaycastHit()
                            {
                                EntityID = collider.EntityID,
                                Intersection = intersection
                            };

                            shortestDistance = distance;
                        }
                    }
                }
            }

            return (hit.EntityID != 0);
        }

        public static bool TryCircleCast(RayCircle rayCircle, IEnumerable<PhysicsBody> colliders, IEntityProvider entityProvider, out RaycastHit hit)
        {
            foreach (var collider in colliders)
            {
                var position = entityProvider.GetEntity(collider.EntityID).Position;

                if (collider.Shape.GetType() == typeof(Box))
                {
                    var box = (Box)collider.Shape;

                    if (rayCircle.TryGetBoxIntersection(box, position, out Vector3 intersection))
                    {
                        hit = new RaycastHit()
                        {
                            EntityID = collider.EntityID,
                            Intersection = intersection
                        };

                        return true;
                    }
                }
                else if (collider.Shape.GetType() == typeof(Circle))
                {
                    var circle = (Circle)collider.Shape;

                    if (rayCircle.TryGetCircleIntersection(circle, position, out Vector3 intersection))
                    {
                        hit = new RaycastHit()
                        {
                            EntityID = collider.EntityID,
                            Intersection = intersection
                        };

                        return true;
                    }
                }
            }

            hit = new RaycastHit();
            return false;
        }
    }
}
