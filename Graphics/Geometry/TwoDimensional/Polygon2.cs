using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Geometry.TwoDimensional
{
    public class Polygon2
    {
        public List<Vector2> Vertices { get; } = new List<Vector2>();

        public Polygon2(params Vector2[] vertices)
        {
            Vertices.AddRange(vertices);
        }

        public double Area
        {
            get
            {
                double area = 0;

                for (var i = 0; i < Vertices.Count; i++)
                {
                    area += Vertices[i].X * Vertices[(i + 1) % Vertices.Count].Y - Vertices[(i + 1) % Vertices.Count].X * Vertices[i].Y;
                }

                return 0.5 * area;
            }
        }

        public double Perimeter
        {
            get
            {
                double perimeter = 0;

                for (var i = 0; i < Vertices.Count; i++)
                {
                    var lineSegment = new LineSegment2()
                    {
                        Start = Vertices[i],
                        End = Vertices[(i + 1) % Vertices.Count]
                    };

                    perimeter += lineSegment.Length;
                }

                return perimeter;
            }
        }
    }
}
