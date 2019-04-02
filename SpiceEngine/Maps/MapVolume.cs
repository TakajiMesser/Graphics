using OpenTK;
using OpenTK.Graphics;
using SpiceEngine.Entities.Volumes;
using SpiceEngine.Physics.Shapes;
using SpiceEngine.Rendering.Meshes;
using System.Collections.Generic;

namespace SpiceEngine.Maps
{
    public class MapVolume : MapEntity3D<Volume>
    {
        public enum VolumeTypes
        {
            Blocking,
            Physics,
            Trigger
        }

        public VolumeTypes VolumeType { get; set; }
        public Vector3 Gravity { get; set; }

        public List<Vector3> Vertices { get; set; } = new List<Vector3>();
        public List<int> TriangleIndices { get; set; } = new List<int>();
        public Color4 Color { get; set; }

        /*public Mesh3D<Vertex3D> ToMesh()
        {
            var vertices = Vertices.Select(v => new Vertex3D(v, v, v, Vector2.Zero, Color4.Blue)).ToList();
            return new Mesh3D<Vertex3D>(vertices, TriangleIndices);
        }*/

        public override Volume ToEntity()
        {
            switch (VolumeType)
            {
                case VolumeTypes.Blocking:
                    return new BlockingVolume()//Vertices, TriangleIndices, Color)
                    {
                        Position = Position,
                        Rotation = Quaternion.FromEulerAngles(Rotation),
                        Scale = Scale
                    };
                case VolumeTypes.Physics:
                    return new PhysicsVolume()//Vertices, TriangleIndices, Color)
                    {
                        Position = Position,
                        Rotation = Quaternion.FromEulerAngles(Rotation),
                        Scale = Scale,
                        Gravity = Gravity
                    };
                case VolumeTypes.Trigger:
                    return new TriggerVolume()//Vertices, TriangleIndices, Color)
                    {
                        Position = Position,
                        Rotation = Quaternion.FromEulerAngles(Rotation),
                        Scale = Scale
                    };
            }

            return new Volume()//Vertices, TriangleIndices, Color)
            {
                Position = Position,
                Rotation = Quaternion.FromEulerAngles(Rotation),
                Scale = Scale
            };
        }

        public Shape3D ToShape() => new Box(Vertices);

        public static MapVolume Rectangle(Vector3 center, float width, float height)
        {
            var meshShape = MeshShape.Rectangle(width, height);

            return new MapVolume()
            {
                Position = center,
                Vertices = meshShape.Vertices,
                TriangleIndices = meshShape.TriangleIndices
            };
        }

        public static MapVolume Box(Vector3 center, float width, float height, float depth)
        {
            var meshShape = MeshShape.Box(width, height, depth);

            var vertices = new List<Vector3>();
            var triangleIndices = new List<int>();

            // TODO - This shouldn't be necessary anymore, since Volume's don't have UV coordinates to calculate
            for (var i = 0; i < meshShape.TriangleIndices.Count; i++)
            {
                // Grab vertexIndices, three at a time, to form each triangle
                if (i % 3 == 0)
                {
                    // For a given triangle with vertex positions P0, P1, P2 and corresponding UV texture coordinates T0, T1, and T2:
                    // deltaPos1 = P1 - P0;
                    // delgaPos2 = P2 - P0;
                    // deltaUv1 = T1 - T0;
                    // deltaUv2 = T2 - T0;
                    // r = 1 / (deltaUv1.x * deltaUv2.y - deltaUv1.y - deltaUv2.x);
                    // tangent = (deltaPos1 * deltaUv2.y - deltaPos2 * deltaUv1.y) * r;
                    var deltaPos1 = meshShape.Vertices[meshShape.TriangleIndices[i + 1] - 1] - meshShape.Vertices[meshShape.TriangleIndices[i] - 1];
                    var deltaPos2 = meshShape.Vertices[meshShape.TriangleIndices[i + 2] - 1] - meshShape.Vertices[meshShape.TriangleIndices[i] - 1];
                }

                var meshVertex = meshShape.Vertices[meshShape.TriangleIndices[i] - 1];
                var existingIndex = vertices.FindIndex(v => v == meshVertex);

                if (existingIndex >= 0)
                {
                    triangleIndices.Add(existingIndex);
                }
                else
                {
                    triangleIndices.Add(vertices.Count);
                    vertices.Add(meshVertex);
                }
            }

            return new MapVolume()
            {
                Position = center,
                Vertices = vertices,
                TriangleIndices = triangleIndices
            };
        }
    }
}
