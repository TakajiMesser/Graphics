using Graphics.GameObjects;
using Graphics.Helpers;
using Graphics.Materials;
using Graphics.Meshes;
using Graphics.Physics.Collision;
using Graphics.Rendering.Shaders;
using Graphics.Rendering.Vertices;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Graphics.Maps
{
    public class MapBrush
    {
        public List<Vertex> Vertices { get; set; } = new List<Vertex>();
        public List<Material> Materials { get; set; } = new List<Material>();
        public List<int> TriangleIndices { get; set; } = new List<int>();
        public bool HasCollision { get; set; }

        public Brush ToBrush(ShaderProgram program)
        {
            var brush = new Brush(Vertices, Materials, TriangleIndices, program)
            {
                HasCollision = HasCollision
            };
            brush.Collider = new BoundingBox(brush);
            
            return brush;
        }

        public static MapBrush Rectangle(Vector3 center, float width, float height)
        {
            return new MapBrush()
            {
                Vertices = new List<Vertex>
                {
                    new Vertex(new Vector3(center.X - width / 2.0f, center.Y - height / 2.0f, center.Z), Vector3.UnitZ, Vector2.Zero, 0),
                    new Vertex(new Vector3(center.X - width / 2.0f, center.Y + height / 2.0f, center.Z), Vector3.UnitZ, Vector2.Zero, 0),
                    new Vertex(new Vector3(center.X + width / 2.0f, center.Y - height / 2.0f, center.Z), Vector3.UnitZ, Vector2.Zero, 0),
                    new Vertex(new Vector3(center.X + width / 2.0f, center.Y + height / 2.0f, center.Z), Vector3.UnitZ, Vector2.Zero, 0)
                },
                Materials = new List<Material>
                {
                    Material.LoadFromFile(FilePathHelper.GENERIC_MATERIAL_PATH).First().Item2
                },
                TriangleIndices = new List<int>()
                {
                    0, 1, 2, 1, 2, 3
                }
            };
        }
    }
}
