using OpenTK;
using SpiceEngine.Physics.Collisions;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Physics.Shapes
{
    public class Tetrahedron : ISimplex
    {
        public Vector3 VertexA { get; private set; }
        public Vector3 VertexB { get; private set; }
        public Vector3 VertexC { get; private set; }
        public Vector3 VertexD { get; private set; }

        public Tetrahedron()
        {

        }
        
        public void Add(Vector3 vertex)
        {
            if (VertexA == null)
            {
                VertexA = vertex;
            }
            else if (VertexB == null)
            {
                VertexB = vertex;
            }
            else if (VertexC == null)
            {
                VertexC = vertex;
            }
            else if (VertexD == null)
            {
                VertexD = vertex;
            }
        }

        public Vector3 GetLastVertex()
        {
            if (VertexD != null)
            {
                return VertexD;
            }
            else if (VertexC != null)
            {
                return VertexC;
            }
            else if (VertexB != null)
            {
                return VertexB;
            }
            else if (VertexA != null)
            {
                return VertexA;
            }
        }

        public Vector3 GetPointClosestToOrigin()
        {
            if (VertexD != null)
            {
                return GetPointOnTetrahedronClosestToOrigin();
            }
            else if (VertexC != null)
            {
                return GetPointOnTriangleClosestToOrigin();
            }
            else if (VertexB != null)
            {
                return GetPointOnSegmentClosestToOrigin();
            }
            else if (VertexA != null)
            {
                return VertexA;
            }
            else
            {
                return Vector3.Zero;
            }
        }

        private Vector3 GetPointOnSegmentClosestToOrigin()
        {
            var displacement = VertexB - VertexA;
            var dotA = Vector3.Dot(displacement, VertexA);

            var v = -dotA / displacement.LengthSquared;
            return v * displacement + VertexA;
        }

        private Vector3 GetPointOnTriangleClosestToOrigin()
        {
            var ab = VertexB - VertexA;
            var ac = VertexC - VertexA;

            // Check if outside C
            var d5 = -Vector3.Dot(ab, VertexC);
            var d6 = -Vector3.Dot(ac, VertexC);

            if (d6 >= 0f && d5 <= d6)
            {
                // It is C (?!)
                VertexA = VertexC;
                VertexB = null;
                VertexC = null;

                return VertexA;
            }

            // Check if outside AC
            var d1 = -Vector3.Dot(ab, VertexA);
            var d2 = -Vector3.Dot(ac, VertexA);
            var vb = d5 * d2 - d1 * d6;
            
            if (vb <= 0f && de > 0f && d6 < 0f)
            {
                // Get rid of B and compress C into B
                VertexB = VertexC;
                VertexC = null;

                var v = d2 / (d2 - d6);
                return ac * v + VertexA;
            }

            // Check if outside BC
            var d3 = -Vector3.Dot(ab, VertexB);
            var d4 = -Vector3.Dot(ac, VertexB);
            var va = d3 * d6 - d5 * d4;
            var d3d4 = d4 - d3;
            var d6d5 = d5 - d6;

            if (va <= 0f && d3d4 > 0f && d6d5 > 0f)
            {
                // Get rid of A and compress C into A
                VertexA = VertexC;
                VertexC = null;

                var u = d3d4 / (d3d4 + d6d5);
                return u * (VertexC - VertexB) + VertexB;
            }

            // It is on the face of the triangle
            var vc = d1 * d4 - d3 * d2;
            var denominator = 1f / (va + vb + vc);
            var v2 = vb * denominator;
            var w = vc * denominator;

            var acw = w * ac;
            return VertexA + v2 * ab + acw;
        }

        private Vector3 GetPointOnTetrahedronClosestToOrigin()
        {
            // Because we know that VertexD is new, we can ignore voronoi regions A, B, C, AC, AB, BC, ABC and only consider D, DA, DB, DC, DAC, DCB, DBA
            var minimumSimplex = new Tetrahedron();

            Tetrahedron candidateSimplex;
            Vector3 candidatePoint;

            if (TryTetrahedronTriangle(VertexA, VertexC, VertexD, VertexB, out candidateSimplex, out candidatePoint))
            {
                
            }
        }

        private bool TryTetrahedronTriangle(Vector3 vertexA, Vector3 vertexB, Vector3 vertexC, Vector3 excludedPoint, out Tetrahedron simplex, out Vector3 point)
        {
            var simplex = new SimpleSimplex();
            var point = new Vector3();

            var ab = vertexB - vertexA;
            var ac = vertexC - vertexA;
            var normal = Vector3.Cross(ab, ac);
            var ad = excludedPoint - A;
            var aDotN = Vector3.Dot(vertexA, normal);
            var adDotN = Vector3.Dot(ad, normal);

            // If (-A * N) * (AD * N) < 0, D and the origin are on opposite sides of the triangle
            if (aDotN * adDotN > 0)
            {
                // Rather than storing vector AP, since P is the origin, just use -A (same for B and C)

                // Check to see if it's outside C
                var cDotAB = -Vector3.Dot(ab, vertexC);
                var cDotAC = -Vector3.Dot(ac, vertexC);

                if (cDotAC >= 0f && cDotAB <= cDotAC)
                {
                    // It is C
                    simplex.Add(vertexC);
                    point = vertexC;

                    return true;
                }

                // Check if outside AC
                var aDotAB = -Vector3.Dot(ab, vertexA);
                var aDotAC = -Vector3.Dot(ac, vertexA);

                var vb = cDotAB * aDotAC - aDotAB * cDotAC;
                if (vb <= 0f && aDotAC > 0f && cDotAC < 0f)
                {
                    simplex.Add(vertexA);
                    simplex.Add(vertexC);

                    var v = aDotAC / (aDotAC - cDotAC);
                    point = v * ac + vertexA;

                    return true;
                }

                // Check if outside BC
                var bDotAB = -Vector3.Dot(ab, vertexB);
                var bDotAC = -Vector3.Dot(ac, vertexB);

                var va = bDotAB * cDotAC - cDotAB * bDotAC;
                var d3d4 = bDotAC - bDotAB;
                var d6d5 = cDotAB - cDotAC;

                if (va <= 0f && d3d4 > 0f && d6d5 > 0f)
                {
                    simplex.Add(vertexB);
                    simplex.Add(vertexC);

                    var v = d3d4 / (d3d4 + d6d5);
                    point = v * (c - b) + vertexB;

                    return true;
                }

                // It is on the face of the triangle
                simplex.Add(vertexA);
                simplex.Add(vertexB);
                simplex.Add(vertexC);

                var vc = aDotAB * bDotAC - bDotAB * aDotAC;
                var denominator = 1f / (va + vb + vc);
                var v2 = vb * denominator;
                var w = vc * denominator;

                var acw = w * ac;
                point = vertexA + v2 * ab + acw;

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
