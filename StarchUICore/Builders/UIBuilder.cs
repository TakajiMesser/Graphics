using OpenTK;
using OpenTK.Graphics;
using SpiceEngineCore.Rendering.Vertices;
using System;
using System.Collections.Generic;

namespace StarchUICore.Builders
{
    public class UIBuilder
    {
        public static Vertex3DSet<ViewVertex> Rectangle(float width, float height, Color4 color)
        {
            var vertices = new List<ViewVertex>
            {
                new ViewVertex(new Vector3(0.0f, 0.0f, 0.0f), color, Color4.PaleVioletRed),
                new ViewVertex(new Vector3(0.0f, height, 0.0f), color, Color4.PaleVioletRed),
                new ViewVertex(new Vector3(width, height, 0.0f), color, Color4.PaleVioletRed),
                new ViewVertex(new Vector3(width, 0.0f, 0.0f), color, Color4.PaleVioletRed)
            };

            var triangleIndices = new List<int>{ 2, 1, 0, 3, 2, 0 };

            return new Vertex3DSet<ViewVertex>(vertices, triangleIndices);
        }

        public static Vertex3DSet<ViewVertex> RoundedRectangle(float width, float height, float radius, int nSides, Color4 color)
        {
            // B A
            // C D
            var vertices = new List<ViewVertex>
            {
                new ViewVertex(new Vector3(0.0f, 0.0f, 0.0f), color, Color4.PaleVioletRed),
                new ViewVertex(new Vector3(0.0f, height, 0.0f), color, Color4.PaleVioletRed),
                new ViewVertex(new Vector3(width, height, 0.0f), color, Color4.PaleVioletRed),
                new ViewVertex(new Vector3(width, 0.0f, 0.0f), color, Color4.PaleVioletRed)
            };

            var triangleIndices = new List<int> { 2, 1, 0, 3, 2, 0 };

            var cornerPointA = new Vector2(width - radius, radius);
            var quadrantAPoints = GetRoundedRectangleQuadrant(cornerPointA, radius, nSides, 0);

            foreach (var point in quadrantAPoints)
            {
                vertices.Add(new ViewVertex(new Vector3(point.X, point.Y, 0.0f), color, Color4.PaleVioletRed));
            }

            var cornerPointB = new Vector2(radius, radius);
            var quadrantBPoints = GetRoundedRectangleQuadrant(cornerPointB, radius, nSides, 1);

            foreach (var point in quadrantBPoints)
            {
                vertices.Add(new ViewVertex(new Vector3(point.X, point.Y, 0.0f), color, Color4.PaleVioletRed));
            }

            var cornerPointC = new Vector2(radius, height - radius);
            var quadrantCPoints = GetRoundedRectangleQuadrant(cornerPointC, radius, nSides, 2);

            foreach (var point in quadrantCPoints)
            {
                vertices.Add(new ViewVertex(new Vector3(point.X, point.Y, 0.0f), color, Color4.PaleVioletRed));
            }

            var cornerPointD = new Vector2(width - radius, height - radius);
            var quadrantDPoints = GetRoundedRectangleQuadrant(cornerPointD, radius, nSides, 3);

            foreach (var point in quadrantDPoints)
            {
                vertices.Add(new ViewVertex(new Vector3(point.X, point.Y, 0.0f), color, Color4.PaleVioletRed));
            }

            return new Vertex3DSet<ViewVertex>(vertices, triangleIndices);
        }

        private static IEnumerable<Vector2> GetRoundedRectangleQuadrant(Vector2 cornerPoint, float radius, int nSides, int quadrantIndex)
        {
            var previousPoint = new Vector2();

            for (var i = 0; i < nSides; i++)
            {
                var angle = MathHelper.PiOver2 * i / nSides + quadrantIndex * MathHelper.PiOver2;

                var x = radius * (float)Math.Cos(angle);
                var y = radius * (float)Math.Sin(angle);

                var point = new Vector2(cornerPoint.X + x, cornerPoint.Y - y);

                if (i > 0)
                {
                    yield return cornerPoint;
                    yield return previousPoint;
                    yield return point;
                }

                previousPoint = point;
            }
        }
    }
}
