using Graphics.Physics.Collision;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Physics.Raycasting
{
    public static class Raycast
    {
        public static bool TryRaycast(Ray3 ray, IEnumerable<Collider> colliders, out RaycastHit hit)
        {
            foreach (var collider in colliders)
            {
                if (collider.GetType() == typeof(BoundingBox))
                {
                    var box = (BoundingBox)collider;

                    if (ray.TryGetBoxIntersection(box, out Vector3 intersection))
                    {
                        hit = new RaycastHit()
                        {
                            Collider = collider,
                            Intersection = intersection
                        };

                        return true;
                    }
                }
            }

            hit = new RaycastHit();
            return false;
        }

        public static bool TryCircleCast(Circle circle, IEnumerable<Collider> colliders, out RaycastHit hit)
        {
            foreach (var collider in colliders)
            {
                if (collider.GetType() == typeof(BoundingBox))
                {
                    var box = (BoundingBox)collider;

                    if (circle.TryGetBoxIntersection(box, out Vector3 intersection))
                    {
                        hit = new RaycastHit()
                        {
                            Collider = collider,
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
