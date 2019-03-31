using OpenTK;
using SpiceEngine.Physics.Bodies;
using SpiceEngine.Physics.Shapes;

namespace SpiceEngine.Helpers
{
    public static class MinkowskiHelper
    {
        public const int MAX_ITERATIONS = 16;

        // GJK
        public static bool GenerateSimplex(Body3D bodyA, Body3D bodyB)
        {
            var simplex = new Tetrahedron();

            // Choose an initial search direction
            var direction = Vector3.Zero;

            simplex.Add(GetMinkowskiVertex(bodyA, bodyB, -direction));

            for (var i = 0; i < MAX_ITERATIONS; i++)
            {
                var closestPoint = simplex.GetPointClosestToOrigin();
                if (closestPoint.HasValue || closestPoint.Value.LengthSquared <= simplex.GetErrorTolerance())
                {
                    return true;
                }
                else
                {
                    var vertex = GetMinkowskiVertex(bodyA, bodyB, direction);

                    if (Vector3.Dot(vertex, closestPoint.Value) > 0)
                    {
                        return false;
                    }
                    else
                    {
                        simplex.Add(vertex);
                    }
                }
            }

            return false;
        }

        private static Vector3 GetMinkowskiVertex(Body3D bodyA, Body3D bodyB, Vector3 direction)
        {
            var vertexA = bodyA.Position + bodyA.Shape.GetFurthestPointInDirection(direction);
            var vertexB = bodyB.Position + bodyB.Shape.GetFurthestPointInDirection(-direction);

            return vertexA - vertexB;
        }

        /*private static bool ContainsOrigin(Tetrahedron simplex, out Vector3 direction)
        {
            var a = simplex.GetLastVertex();

            var ao = -a;

            if (_vertices.Count == 3)
            {
                // Get B and C of triangle
                var b = getB();
                var c = getC();

                // Compute edges
                var ab = b - a;
                var ac = c - a;

                // Compute the normals
                var abPerp = CalculateTripleProduct(ac, ab, ab);
                var acPerp = CalculateTripleProduct(ab, ac, ac);

                if (Vector3.Dot(abPerp, ao) > 0)
                {
                    // Origin is in R4, so remove C
                    direction = abPerp;
                }
                else if (Vector3.Dot(acPerp, ao) > 0)
                {
                    // Origin is in R3, so remove B
                    direction = acPerp;
                }
                else
                {
                    // Origin is in R5
                    direction = new Vector3();
                    return true;
                }
            }
            else
            {
                var b = getB();

                var ab = b - a;

                // Compute perpendicular to AB in the direction of the origin
                var abPerp = CalculateTripleProduct(ab, ao, ab);
                direction = abPerp;

                return false;
            }
        }*/

        private static Vector3 CalculateTripleProduct(Vector3 vectorA, Vector3 vectorB, Vector3 vectorC) => Vector3.Dot(vectorC, vectorA) * vectorB - Vector3.Dot(vectorC, vectorB) * vectorA;
    }
}
