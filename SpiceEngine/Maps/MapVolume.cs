using OpenTK;
using OpenTK.Graphics;
using SpiceEngine.Entities.Volumes;
using SpiceEngine.Physics.Shapes;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Utilities;
using System.Collections.Generic;
using System.Linq;

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
                        Rotation = Quaternion.FromEulerAngles(Rotation.ToRadians()),
                        Scale = Scale
                    };
                case VolumeTypes.Physics:
                    return new PhysicsVolume()//Vertices, TriangleIndices, Color)
                    {
                        Position = Position,
                        Rotation = Quaternion.FromEulerAngles(Rotation.ToRadians()),
                        Scale = Scale,
                        Gravity = Gravity
                    };
                case VolumeTypes.Trigger:
                    return new TriggerVolume()//Vertices, TriangleIndices, Color)
                    {
                        Position = Position,
                        Rotation = Quaternion.FromEulerAngles(Rotation.ToRadians()),
                        Scale = Scale
                    };
            }

            return new Volume()//Vertices, TriangleIndices, Color)
            {
                Position = Position,
                Rotation = Quaternion.FromEulerAngles(Rotation.ToRadians()),
                Scale = Scale
            };
        }

        public Shape3D ToShape() => new Box(Vertices);

        public static MapVolume Rectangle(Vector3 center, float width, float height)
        {
            var meshShape = new MeshShape();
            meshShape.Faces.Add(MeshFace.Rectangle(width, height));
            var meshBuild = new MeshBuild(meshShape);

            return new MapVolume()
            {
                Position = center,
                Vertices = meshBuild.Positions.ToList(),
                TriangleIndices = meshBuild.TriangleIndices
            };
        }

        public static MapVolume Box(Vector3 center, float width, float height, float depth)
        {
            var meshShape = MeshShape.Box(width, height, depth);
            var meshBuild = new MeshBuild(meshShape);

            return new MapVolume()
            {
                Position = center,
                Vertices = meshBuild.Positions.ToList(),
                TriangleIndices = meshBuild.TriangleIndices
            };
        }
    }
}
